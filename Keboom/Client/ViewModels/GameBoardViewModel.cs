using Keboom.Shared;

namespace Keboom.Client.ViewModels;

public class GameBoardViewModel : ViewModelBase
{

    public GameState? GameState { get; private set; }

    public override Task OnInitializedAsync()
    {

        //TODO swagger client
        return base.OnInitializedAsync();
    }
}
