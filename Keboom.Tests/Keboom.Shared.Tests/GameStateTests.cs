namespace Keboom.Shared.Tests;

public class GameStateTests
{
    [Fact]
    public void CurrentPlayer_WithPlayerChange_RaisesEvent()
    {
        var gameState = new GameState();
        var player1 = new Player { Id = Guid.NewGuid().ToString(), Name = "Buzz", Score = 0 };
        var player2 = new Player { Id = Guid.NewGuid().ToString(), Name = "Andy", Score = 0 };
        gameState.Player1 = player1;
        gameState.Player2 = player2;
        gameState.CurrentPlayer = player1;

        var eventRaised = false;
        gameState.CurrentPlayerChanged += (sender, args) =>
        {
            eventRaised = true;
        };

        gameState.CurrentPlayer = player2;

        Assert.True(eventRaised);
        Assert.Equal(player2, gameState.CurrentPlayer);
    }

    [Fact]
    public void CurrentPlayer_HasNotChanged_EventNotRaised()
    {
        var gameState = new GameState();
        var player1 = new Player { Id = Guid.NewGuid().ToString(), Name = "Buzz", Score = 0 };
        var player2 = new Player { Id = Guid.NewGuid().ToString(), Name = "Andy", Score = 0 };
        gameState.Player1 = player1;
        gameState.Player2 = player2;
        gameState.CurrentPlayer = player1;

        var eventRaised = false;
        gameState.CurrentPlayerChanged += (sender, args) =>
        {
            eventRaised = true;
        };

        gameState.CurrentPlayer = player1;

        Assert.False(eventRaised);
        Assert.Equal(player1, gameState.CurrentPlayer);
    }
}
