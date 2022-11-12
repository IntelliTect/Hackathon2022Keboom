namespace Keboom.Shared;

public class MockClientSideGameHandler : IGameHubClientSideMethods, IGameHubEventHandler 
{
    
    public void GameState(GameState gameState) => Console.WriteLine("gamestate");
    
    public void NewGameId(string gameId) => Console.WriteLine("new game!");
    public void PlayerLeft(string playerId) => Console.WriteLine("playerleft");
    public void StartGame(GameState gameState) => Console.WriteLine("startgame");

    public void Connected() => Console.WriteLine("connected");
    public void LostConnection() => Console.WriteLine("lost connection");
}
