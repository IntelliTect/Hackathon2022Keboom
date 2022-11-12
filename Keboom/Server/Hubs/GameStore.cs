using Keboom.Shared;

namespace Keboom.Server.Hubs;


public class GameStore : IGameStore
{
    private Dictionary<string, GameState> Games { get; } = new();
    
    private readonly Dictionary<string, string> PlayerIdToGameId = new();

    private readonly object newGameLock = new();

    public GameState JoinGame(JoinGameRequest joinRequest)
    {
        lock (newGameLock)
        {
            RemoveFromGame(joinRequest.PlayerId ?? "");
            string gameName = joinRequest.GameName ?? "";
            if (Games.ContainsKey(gameName))
            {
                var existingGame = Games[gameName];

                existingGame.GameStatus = GameStatus.InProgress;
                AddPlayer(existingGame, new Player
                {
                    Id = joinRequest.PlayerId,
                    Name = joinRequest.PlayerName,
                    Color = PlayerColor.Green
                });

                return existingGame;
            }

            var newGame = new GameState
            {
                Id = gameName,
                Board = new Board(joinRequest.BoardWidth, joinRequest.BoardHeight, joinRequest.NumberOfMines),
                GameStatus = GameStatus.WaitingForPlayersToJoin
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
        if (!PlayerIdToGameId.ContainsKey(playerId)){

            return;
        }

        string gameId = PlayerIdToGameId[playerId];
        PlayerIdToGameId.Remove(playerId);

        if (gameId is not null && Games.ContainsKey(gameId))
        {
            Games.Remove(gameId);

            // todo allow player to reconnect
        }
    }

    public string? GetGame(string playerId)
    {
        PlayerIdToGameId.TryGetValue(playerId, out string? game);
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
}

public interface IGameStore
{
    string? GetGame(string playerId);
    void RemoveFromGame(string playerId);
    GameState JoinGame(JoinGameRequest joinRequest);
}
