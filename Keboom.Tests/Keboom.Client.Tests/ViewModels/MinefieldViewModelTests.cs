using Keboom.Client.ViewModels;
using Keboom.Shared.Tests;

namespace Keboom.Client.Tests.ViewModels;

public class MinefieldViewModelTests
{
    [Fact]
    public void OpenCell_WithMine_RemainsCurrentPlayer()
    {
        AutoMocker mocker = new();
        
        MinefieldViewModel viewModel = mocker.CreateInstance<MinefieldViewModel>();
        viewModel.GameState = GameStateBuilder.CreateNewGame();
        viewModel.GameState.CurrentPlayer = viewModel.GameState.Player1;

        Board board = viewModel.GameState.Board!;
        BoardSpace mineSpace = board.FlatGrid.First(x => x.HasMine && x.ClaimedByPlayer is null);

        viewModel.OpenCellCommand.Execute(mineSpace);

        Assert.Equal(viewModel.GameState.Player1, viewModel.GameState.CurrentPlayer);
    }

    [Fact]
    public void OpenCell_WithoutMine_MovesToNextPlayer()
    {
        AutoMocker mocker = new();

        MinefieldViewModel viewModel = mocker.CreateInstance<MinefieldViewModel>();
        viewModel.GameState = GameStateBuilder.CreateNewGame();
        viewModel.GameState.CurrentPlayer = viewModel.GameState.Player1;

        Board board = viewModel.GameState.Board!;
        BoardSpace mineSpace = board.FlatGrid.First(x => !x.HasMine && x.ClaimedByPlayer is null);

        viewModel.OpenCellCommand.Execute(mineSpace);

        Assert.Equal(viewModel.GameState.Player2, viewModel.GameState.CurrentPlayer);
    }
}
