namespace Keboom.Shared.Tests;

public class GameStateTests
{
    [Fact]
    public void CurrentPlayer_WithPlayerChange_RaisesEvent()
    {
        GameState gameState = GameStateBuilder.CreateNewGame();
        gameState.CurrentPlayer = gameState.Player1;

        var eventRaised = false;
        gameState.CurrentPlayerChanged += (sender, args) =>
        {
            eventRaised = true;
        };

        gameState.CurrentPlayer = gameState.Player2;

        Assert.True(eventRaised);
        Assert.Equal(gameState.Player2, gameState.CurrentPlayer);
    }

    [Fact]
    public void CurrentPlayer_HasNotChanged_EventNotRaised()
    {
        GameState gameState = GameStateBuilder.CreateNewGame();
        gameState.CurrentPlayer = gameState.Player1;

        var eventRaised = false;
        gameState.CurrentPlayerChanged += (sender, args) =>
        {
            eventRaised = true;
        };

        gameState.CurrentPlayer = gameState.Player1;

        Assert.False(eventRaised);
        Assert.Equal(gameState.Player1, gameState.CurrentPlayer);
    }
}
