using Keboom.Shared;

namespace Keboom.Server.Hubs;


public class GameStore : IGameStore
{
    private readonly Dictionary<string, string> PlayerIdToGameId= new();
    private readonly Dictionary<string, List<Player>> GameToPlayers = new();


    public void AddToGame(string playerId, string gameId, string playerName) {
        PlayerIdToGameId.Add(playerId, gameId);

        GameToPlayers.TryGetValue(gameId, out List<Player>? players);

        var newPlayer = new Player() { Id = playerId, Name = playerName };

        if (players is null) {
            GameToPlayers[gameId] = new List<Player>() { newPlayer };
        }
        else {
            players.Add(newPlayer);
        }
        
    }

    public int GamePlayerCount(string gameId) => GameToPlayers[gameId]?.Count ?? 0;

    public string? GetGame(string playerId) {

       PlayerIdToGameId.TryGetValue(playerId, out string? game);
        return game;
    }

    public List<Player> GetGamePlayers(string gameId) {
        return GameToPlayers[gameId] ?? new List<Player>();
    }

    public void RemoveFromGame(string playerId)
    {

        string? gameId = PlayerIdToGameId[playerId];
        PlayerIdToGameId.Remove(playerId);
        if (gameId is not null)
        {
            List<Player>? players = GameToPlayers[gameId];
            if (players is not null) {
                players.RemoveAll(p=> p.Id == playerId);
                GameToPlayers[gameId] = players;
                if (players.Count == 0) {
                    GameToPlayers.Remove(gameId);
                }
            }
        }
    }
}

public interface IGameStore
{
    string? GetGame(string playerId);
    void AddToGame(string playerId, string gameId, string playerName);
    void RemoveFromGame(string playerId);
    int GamePlayerCount(string gameId);
    List<Player> GetGamePlayers(string gameId);
}
