namespace Keboom.Shared.Tests;

public static class GameStateBuilder
{
    public static GameState CreateNewGame(
        int boardWidth = 3,
        int boardHeight = 3,
        int numberOfMines = 2)
    {
        var gameState = new GameState();
        var player1 = new Player { Id = Guid.NewGuid().ToString(), Name = "Buzz", Score = 0 };
        var player2 = new Player { Id = Guid.NewGuid().ToString(), Name = "Andy", Score = 0 };
        gameState.Player1 = player1;
        gameState.Player2 = player2;
        gameState.CurrentPlayer = player1;
        
        gameState.Board = new Board(boardWidth, boardHeight, numberOfMines);

        return gameState;
    }
}
