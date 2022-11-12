using Keboom.Shared;

namespace Keboom.Client.ViewModels;

public class GameBoardViewModel : ViewModelBase
{

    public GameState? GameState { get; private set; }

    public override Task OnInitializedAsync()
    {
        var bg = new BoardGenerator();
        GameState = new GameState
        {
            Board = bg.CreateBoard(10, 10, 22),
            Player1 = new Player { Id = int.MaxValue, Name = "Player 1", Score = 0 },
            Player2 = new Player { Id = int.MinValue, Name = "Player 2", Score = 0 }
        };

        GameState.CurrentPlayer = GameState.Player1;
        return base.OnInitializedAsync();
    }
}
