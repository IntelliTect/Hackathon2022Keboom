namespace Keboom.Shared.Tests;

public class GameStateTests
{
    [Fact]
    public void NextPlayersTurn_ShouldSetCurrentPlayerToPlayer2_WhenCurrentPlayerIsPlayer1()
    {
        // Arrange
        var gameState = new GameState
        {
            Player1 = new Player(),
            Player2 = new Player(),
            CurrentPlayer = new Player()
        };
        gameState.Players[0] = gameState.Player1;
        gameState.Players[1] = gameState.Player2;
        gameState.CurrentPlayer = gameState.Player1;

        // Act
        gameState.NextPlayersTurn();

        // Assert
        Assert.Equal(gameState.Player2, gameState.CurrentPlayer);
    }

    [Fact]
    public void NextPlayersTurn_ShouldSetCurrentPlayerToPlayer1_WhenCurrentPlayerIsPlayer2()
    {
        // Arrange
        var gameState = new GameState
        {
            Player1 = new Player(),
            Player2 = new Player(),
            CurrentPlayer = new Player()
        };
        gameState.Players[0] = gameState.Player1;
        gameState.Players[1] = gameState.Player2;
        gameState.CurrentPlayer = gameState.Player2;

        // Act
        gameState.NextPlayersTurn();

        // Assert
        Assert.Equal(gameState.Player1, gameState.CurrentPlayer);
    }
}
