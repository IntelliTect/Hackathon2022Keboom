using Keboom.Shared;

namespace Keboom.Server.Hubs;


public class GameStore : IGameStore
{
    private Dictionary<string, GameState> Games { get; } = new();

    private readonly Dictionary<string, string> PlayerConnectionIdToGameId = new();
    private readonly Dictionary<string, string> PlayerIdToPlayerConnectionId = new();

    private readonly object newGameLock = new();

    public GameState JoinGame(JoinGameRequest joinRequest)
    {
        lock (newGameLock)
        {
            RemoveFromGame(joinRequest.PlayerId ?? "");
            string gameName = joinRequest.GameName ?? "";

            if (joinRequest.IsPublic)
            {
                var existingPublicGame = FindAvailablePublicGame();

                if(existingPublicGame is not null)
                {

                    return JoinExistingGame(existingPublicGame, joinRequest);
                }
            }

            if (Games.ContainsKey(gameName))
            {
                var existingGame = Games[gameName];
                return JoinExistingGame(existingGame, joinRequest);
            }

            var newGame = new GameState
            {
                Id = gameName,
                Board = new Board(joinRequest.BoardWidth, joinRequest.BoardHeight, joinRequest.NumberOfMines),
                GameStatus = GameStatus.WaitingForPlayersToJoin,
                IsPublic = joinRequest.IsPublic
            };

            AddPlayer(newGame, new Player
            {
                Id = joinRequest.PlayerId,
                Name = joinRequest.PlayerName,
                Color = PlayerColor.Red
            });
            Games[newGame.Id] = newGame;

            return newGame;
        }
    }

    public void RemoveFromGame(string playerId)
    {
        if (!PlayerConnectionIdToGameId.ContainsKey(playerId))
        {

            return;
        }

        string gameId = PlayerConnectionIdToGameId[playerId];
        PlayerConnectionIdToGameId.Remove(playerId);

        if (gameId is not null && Games.ContainsKey(gameId))
        {
            Games.Remove(gameId);

            // todo allow player to reconnect
        }
    }

    public string? GetGame(string playerId)
    {
        PlayerConnectionIdToGameId.TryGetValue(playerId, out string? game);
        return game;
    }

    public static void AddPlayer(GameState game, Player player)
    {
        if (game.Players.FirstOrDefault(p => p.Id == player.Id) is not null)
        {
            return;
        }

        if (game.Players.Count >= 2)
        {
            throw new InvalidOperationException("Can't add more than two players");
        }

        game.Players.Add(player);

        if (game.CurrentPlayer is null)
        {
            game.CurrentPlayer = player;
        }
    }

    private GameState JoinExistingGame(GameState existingGame, JoinGameRequest joinRequest)
    {
        existingGame.GameStatus = GameStatus.InProgress;
        AddPlayer(existingGame, new Player
        {
            Id = joinRequest.PlayerId,
            Name = joinRequest.PlayerName,
            Color = PlayerColor.Green
        });

        return existingGame;
    }

    private GameState? FindAvailablePublicGame()
    {
        return Games.FirstOrDefault(x => x.Value.IsPublic && x.Value.Players.Count == 1).Value;
    }

    public void AddPlayerConnectionID(string gameId, string playerId, string playerConnectionId) {
        PlayerIdToPlayerConnectionId.Remove(playerId);
        PlayerIdToPlayerConnectionId.Add(playerId, playerConnectionId);
        PlayerConnectionIdToGameId.Remove(playerConnectionId);
        PlayerConnectionIdToGameId.Add(playerConnectionId, gameId);
    }
}

public interface IGameStore
{
    string? GetGame(string playerId);
    void RemoveFromGame(string playerId);
    GameState JoinGame(JoinGameRequest joinRequest);
    void AddPlayerConnectionID(string gameId, string playerId, string playerConnectionId);
}
