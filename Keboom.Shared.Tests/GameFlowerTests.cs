using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keboom.Shared.Tests;

public class GameFlowerTests
{
    [Fact]
    public void TriggerSapce_Unclaimed_FlaggedToCorrectPlayer()
    {
        var player = new Player
        {
            Id = Guid.NewGuid()
        };

        var gameState = CreateGameState(true);

        var gameFlower = new GameFlower(gameState, player);

        gameFlower.TriggerSpace(gameState.Board!.Grid[0,0]);

        Assert.Equal(player.Id, gameState.Board.Grid[0, 0].ClaimedByPlayer);
    }

    [Fact]
    public void TriggerSapce_Claimed_FlagNotStolen()
    {
        var player = new Player
        {
            Id = Guid.NewGuid()
        };

        var player2Id = Guid.NewGuid();

        var gameState = CreateGameState(false);

        var gameFlower = new GameFlower(gameState, player);

        gameState.Board!.Grid[0, 0].ClaimedByPlayer = player2Id;

        gameFlower.TriggerSpace(gameState.Board!.Grid[0, 0]);

        Assert.Equal(player2Id, gameState.Board.Grid[0, 0].ClaimedByPlayer);
    }

    [Fact]
    public void TriggerSapce_UnclaimedMine_ScoreAdded()
    {
        var player = new Player
        {
            Id = Guid.NewGuid()
        };

        var gameState = CreateGameState(true);

        var gameFlower = new GameFlower(gameState, player);

        gameFlower.TriggerSpace(gameState.Board!.Grid[0, 0]);

        Assert.Equal(1, player.Score);
    }

    [Fact]
    public void TriggerSapce_ClaimedMine_ScoreUnchanged()
    {
        var player = new Player
        {
            Id = Guid.NewGuid()
        };

        var gameState = CreateGameState(true);

        var gameFlower = new GameFlower(gameState, player);

        var player2Id = Guid.NewGuid();

        gameState.Board!.Grid[0, 0].ClaimedByPlayer = player2Id;

        gameFlower.TriggerSpace(gameState.Board!.Grid[0, 0]);

        Assert.Equal(0, player.Score);
    }

    [Fact]
    public void TriggerSapce_UnclaimedMine_True()
    {
        var player = new Player
        {
            Id = Guid.NewGuid()
        };

        var gameState = CreateGameState(true);

        var gameFlower = new GameFlower(gameState, player);

        var canContinue = gameFlower.TriggerSpace(gameState.Board!.Grid[0, 0]);

        Assert.True(canContinue);
    }

    [Fact]
    public void TriggerSapce_UnclaimedNoMine_False()
    {
        var player = new Player
        {
            Id = Guid.NewGuid()
        };

        var gameState = CreateGameState(false);

        var gameFlower = new GameFlower(gameState, player);

        var canContinue = gameFlower.TriggerSpace(gameState.Board!.Grid[0, 0]);

        Assert.False(canContinue);
    }

    private GameState CreateGameState(bool hasMines)
    {
        var gameState = new GameState();
        var board = new Board(2, 2);

        for (var x = 0; x < board.Width; x++)
        {
            for (var y = 0; y < board.Height; y++)
            {
                board.Grid[x, y].HasMine = hasMines;
            }
        }

        gameState.Board= board;

        return gameState;
    }
}
