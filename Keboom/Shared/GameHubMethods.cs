namespace Keboom.Shared;

public interface IGameHubClientSideMethods
{
    event EventHandler<EventArgs<string>> PlayerLeft;
    event EventHandler<EventArgs<string>> NewGameId;
    event EventHandler<EventArgs<GameState>> GameStarted;
    event EventHandler<EventArgs<GameState>> GameStateUpdated;
}

public record EventArgs<T>(T Value);

public interface IGameHubServerSideMethods
{
    Task GameState(GameState gameState);
    Task LeaveGame();
    Task JoinGame(string gameId, string playername);
    Task CreateGame(string playerName);
}
