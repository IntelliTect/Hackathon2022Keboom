using Keboom.Shared;

namespace Keboom.Client.ViewModels;

public class GameBoardViewModel : ViewModelBase
{

    public GameState? GameState { get; private set; }

    public override Task OnInitializedAsync()
    {
        GameState = new GameState
        {
            Board = BoardGenerator.CreateBoard(8, 8, 22),
            Player1 = new Player { Id = new Guid(), Name = "Player 1", Score = 0 },
            Player2 = new Player { Id = new Guid(), Name = "Player 2", Score = 0 }
        };

        GameState.CurrentPlayer = GameState.Player1;
        return base.OnInitializedAsync();
    }
}
