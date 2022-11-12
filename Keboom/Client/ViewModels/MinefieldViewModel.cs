
namespace Keboom.Client.ViewModels;

public partial class MinefieldViewModel : ViewModelBase
{
    [ObservableProperty]
    private GameState? _gameState;

    [RelayCommand]
    private void OpenCell(BoardSpace space)
    {
        if (GameState is { } gameState &&
           gameState.CurrentPlayer is { } currentPlayer)
        {
            GameEngine gameEngine = new(gameState, currentPlayer);
            if (gameEngine.TriggerSpace(space))
            {
                NotifyStateChanged();
            }
            else
            {
                gameEngine.NextPlayersTurn();
            }
        }
    }
}
