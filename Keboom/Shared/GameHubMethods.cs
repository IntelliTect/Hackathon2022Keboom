namespace Keboom.Shared;

public interface IGameHubClientSideMethods
{
    event EventHandler<EventArgs> PlayerLeft;
    event EventHandler<EventArgs<GameState>> GameStateUpdated;
}

public record EventArgs<T>(T Value);

public interface IGameHubServerSideMethods
{
    Task GameState(GameState gameState);
    Task JoinGame(string gameId, string playerId);
    Task LeaveGame();
}
