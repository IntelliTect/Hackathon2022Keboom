namespace Keboom.Shared;

public interface IGameHubClientSideMethods
{
    event EventHandler<EventArgs<string>> PlayerLeft;
    event EventHandler<EventArgs<GameState>> GameStateUpdated;
}

public record EventArgs<T>(T Value);

public interface IGameHubServerSideMethods
{
    Task GameState(GameState gameState);
    Task JoinGame(string gameId);
    Task LeaveGame();
}
