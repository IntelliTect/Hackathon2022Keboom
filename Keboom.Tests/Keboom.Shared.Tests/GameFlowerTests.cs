namespace Keboom.Shared.Tests;

public class GameFlowerTests
{
    [Fact]
    public void TriggerSapce_Unclaimed_FlaggedToCorrectPlayer()
    {
        var player = new Player
        {
            Id = Guid.NewGuid().ToString()
        };

        var gameState = CreateGameState(true);

        var gameFlower = new GameFlower(gameState, player);

        gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.Equal(player.Id, gameState.Board[0, 0].ClaimedByPlayer);
    }

    [Fact]
    public void TriggerSapce_Claimed_FlagNotStolen()
    {
        var player = new Player
        {
            Id = Guid.NewGuid().ToString()
        };

        var player2Id = Guid.NewGuid().ToString();

        var gameState = CreateGameState(false);

        var gameFlower = new GameFlower(gameState, player);

        gameState.Board![0, 0].ClaimedByPlayer = player2Id;

        gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.Equal(player2Id, gameState.Board[0, 0].ClaimedByPlayer);
    }

    [Fact]
    public void TriggerSapce_UnclaimedMine_ScoreAdded()
    {
        var player = new Player
        {
            Id = Guid.NewGuid().ToString()
        };

        var gameState = CreateGameState(true);

        var gameFlower = new GameFlower(gameState, player);

        gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.Equal(1, player.Score);
    }

    [Fact]
    public void TriggerSapce_ClaimedMine_ScoreUnchanged()
    {
        var player = new Player
        {
            Id = Guid.NewGuid().ToString()
        };

        var gameState = CreateGameState(true);

        var gameFlower = new GameFlower(gameState, player);

        var player2Id = Guid.NewGuid().ToString();

        gameState.Board![0, 0].ClaimedByPlayer = player2Id;

        gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.Equal(0, player.Score);
    }

    [Fact]
    public void TriggerSapce_UnclaimedMine_True()
    {
        var player = new Player
        {
            Id = Guid.NewGuid().ToString()
        };

        var gameState = CreateGameState(true);

        var gameFlower = new GameFlower(gameState, player);

        var canContinue = gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.True(canContinue);
    }

    [Fact]
    public void TriggerSapce_UnclaimedNoMine_False()
    {
        var player = new Player
        {
            Id = Guid.NewGuid().ToString()
        };

        var gameState = CreateGameState(false);

        var gameFlower = new GameFlower(gameState, player);

        var canContinue = gameFlower.TriggerSpace(gameState.Board![0, 0]);

        Assert.False(canContinue);
    }

    [Fact]
    public void TriggerSapce_UnclaimedZero_OpensAdjacentNonZeros()
    {
        var player = new Player
        {
            Id = Guid.NewGuid().ToString()
        };

        var gameState = CreateGameState(false, 3);
        gameState.Board![1, 2].HasMine = true;
        gameState.Board[2, 1].HasMine = true;
        gameState.Board.SetAdjacentCounts();

        var gameFlower = new GameFlower(gameState, player);

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

    private static GameState CreateGameState(bool hasMines, int size = 2)
    {
        var gameState = new GameState();
        var board = new Board(size, size);

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
