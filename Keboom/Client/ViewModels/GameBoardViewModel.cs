using Keboom.Shared;
using Microsoft.AspNetCore.Components;


namespace Keboom.Client.ViewModels;

public class GameBoardViewModel : ViewModelBase
{

    public GameState? GameState { get; private set; }
    public GameHubConnection GameHubConnection { get; }

    public GameBoardViewModel(GameHubConnection gameHubConnection)
    {
        GameHubConnection = gameHubConnection;
    }

    public override Task OnInitializedAsync()
    {
        GameState = new GameState
        {
            Board = BoardGenerator.CreateBoard(8, 8, 15),
            Player1 = new Player { Id = Guid.NewGuid().ToString(), Name = "Player 1", Score = 0 },
            Player2 = new Player { Id = Guid.NewGuid().ToString(), Name = "Player 2", Score = 0 }
        };

        GameHubConnection.CreateGame("Inigo");
        
        GameState.CurrentPlayer = GameState.Player1;
        return base.OnInitializedAsync();
    }
}
