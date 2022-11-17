namespace Keboom.Shared.Tests;

public class GameEngineTests
{
    [Fact]
    public void TriggerSapce_Unclaimed_FlaggedToCorrectPlayer()
    {
        var gameState = CreateGameState(true);
        Player player = gameState.Players[0];

        GameEngine gameFlower = new(gameState, player);

        gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.Equal(player.Id, gameState.Board[0, 0].ClaimedByPlayer);
    }

    [Fact]
    public void TriggerSapce_Claimed_FlagNotStolen()
    {
        var gameState = CreateGameState(false);
        Player player = gameState.Player1!;
        var player2Id = gameState.Player2!.Id;

        GameEngine gameFlower = new(gameState, player);

        gameState.Board![0, 0].ClaimedByPlayer = player2Id;

        gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.Equal(player2Id, gameState.Board[0, 0].ClaimedByPlayer);
    }

    [Fact]
    public void TriggerSapce_UnclaimedMine_ScoreAdded()
    {
        var gameState = CreateGameState(true);
        Player player = gameState.Players[0];

        GameEngine gameFlower = new(gameState, player);

        gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.Equal(1, player.Score);
    }

    [Fact]
    public void TriggerSapce_ClaimedMine_ScoreUnchanged()
    {
        var gameState = CreateGameState(true);
        Player player = gameState.Players[0]!;

        GameEngine gameFlower = new(gameState, player);

        var player2Id = Guid.NewGuid().ToString();

        gameState.Board![0, 0].ClaimedByPlayer = player2Id;

        gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.Equal(0, player.Score);
    }

    [Fact]
    public void TriggerSapce_UnclaimedMine_True()
    {
        var gameState = CreateGameState(true);
        Player player = gameState.Players[0];

        GameEngine gameFlower = new(gameState, player);

        var canContinue = gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.True(canContinue);
    }

    [Fact]
    public void TriggerSapce_UnclaimedNoMine_False()
    {
        var gameState = CreateGameState(false);
        Player player = gameState.Players[0];

        GameEngine gameFlower = new(gameState, player);

        var canContinue = gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.False(canContinue);
    }

    [Fact]
    public void TriggerSapce_UnclaimedZero_OpensAdjacentNonZeros()
    {
        var gameState = CreateGameState(false, 3);
        gameState.Board![1, 2].HasMine = true;
        gameState.Board[2, 1].HasMine = true;
        gameState.Board.SetAdjacentCounts();
        Player player = gameState.Players[0];

        GameEngine gameFlower = new(gameState, player);

        gameFlower.TriggerSpace(gameState.Board[0, 0]);

        Assert.Equal(player.Id, gameState.Board[0, 0].ClaimedByPlayer);
        Assert.Equal(player.Id, gameState.Board[1, 0].ClaimedByPlayer);
        Assert.Null(gameState.Board[2, 0].ClaimedByPlayer);

        Assert.Equal(player.Id, gameState.Board[0, 1].ClaimedByPlayer);
        Assert.Equal(player.Id, gameState.Board[1, 1].ClaimedByPlayer);
        Assert.Null(gameState.Board[2, 1].ClaimedByPlayer);

        Assert.Null(gameState.Board[0, 2].ClaimedByPlayer);
        Assert.Null(gameState.Board[1, 2].ClaimedByPlayer);
        Assert.Null(gameState.Board[2, 2].ClaimedByPlayer);
    }
    
    [Fact]
    public void NextPlayersTurn_ShouldSetCurrentPlayerToPlayer2_WhenCurrentPlayerIsPlayer1()
    {
        // Arrange
        var gameState = CreateGameState(false, 3);

        GameEngine gameEngine = new(gameState, gameState.Players.First());
        gameState.CurrentPlayer = gameState.Players.First();

        // Act
        gameEngine.NextPlayersTurn();

        // Assert
        Assert.Equal(gameState.Players[1], gameState.CurrentPlayer);
    }

    [Fact]
    public void NextPlayersTurn_ShouldSetCurrentPlayerToPlayer1_WhenCurrentPlayerIsPlayer2()
    {
        // Arrange
        var gameState = CreateGameState(false, 3);

        GameEngine gameEngine = new(gameState, gameState.Player1!);
        gameState.CurrentPlayer = gameState.Player2;

        // Act
        gameEngine.NextPlayersTurn();

        // Assert
        Assert.Equal(gameState.Players[0], gameState.CurrentPlayer);
    }

    [Fact]
    public void NextPlayersTurn_ShouldSetCurrentPlayerToPlayer1_WhenCurrentPlayerIsNull()
    {
        // Arrange
        var gameState = CreateGameState(false, 3);

        GameEngine gameEngine = new(gameState, gameState.Players.First()!);
        gameState.CurrentPlayer = null;

        // Act
        gameEngine.NextPlayersTurn();

        // Assert
        Assert.Equal(gameState.Players[0], gameState.CurrentPlayer);
    }

    private static GameState CreateGameState(bool hasMines, int size = 2, int numMines = 0)
    {
        GameState gameState = GameStateBuilder.CreateNewGame();
        Board board = new(size, size, numMines);

        for (var x = 0; x < board.Width; x++)
        {
            for (var y = 0; y < board.Height; y++)
            {
                board[x, y].HasMine = hasMines;
            }
        }
        board.SetAdjacentCounts();
        gameState.Board = board;

        return gameState;
    }
}
