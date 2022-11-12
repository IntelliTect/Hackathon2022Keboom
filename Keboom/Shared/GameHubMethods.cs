namespace Keboom.Shared;

public interface IGameHubClientSideMethods
{
    void PlayerLeft(string playerId);
    void NewGameId(string gameId);
    void StartGame(GameState gameState);
    void GameState(GameState gameState);

}


public interface IGameHubServerSideMethods
{
    Task GameState(GameState gameState);
    Task LeaveGame();
    Task JoinGame(string gameId, string playername);
    Task CreateGame(string playerName);
}
