﻿using Keboom.Client.ViewModels;

namespace Keboom.Client.Tests.ViewModels;

public class ScoreBoardViewModelTests
{
    [Fact]
    public void WhenGameStateHasPlayers_True()
    {
        AutoMocker mocker = new();

        var sut = mocker.CreateInstance<ScoreBoardViewModel>();

        sut.GameState = new GameState
        {
            Players = {
                new Player { Id = Guid.NewGuid().ToString(), Name = "Buzz", Score = 0 },
                new Player { Id = Guid.NewGuid().ToString(), Name = "Andy", Score = 0 }
            }
        };

        Assert.True(sut.GameHasPlayers);
    }

    [Theory]
    [ClassData(typeof(GameStateTestData))]
    public void WhenGameStateHasNoPlayers_False(GameState? state)
    {
        AutoMocker mocker = new();

        var sut = mocker.CreateInstance<ScoreBoardViewModel>();

        sut.GameState = state;

        Assert.False(sut.GameHasPlayers);
    }
}

public class GameStateTestData : TheoryData<GameState?>
{
    public GameStateTestData()
    {
        Add(new GameState());
        Add(null);
    }
}
