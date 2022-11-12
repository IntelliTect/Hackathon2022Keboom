
namespace Keboom.Client.ViewModels;

public partial class MinefieldViewModel : ViewModelBase
{
    [ObservableProperty]
    private GameState? _gameState;

    public Action? DetonateMines { get; set; }

    [RelayCommand]
    private void OpenCell(BoardSpace space)
    {
        if (GameState is { } gameState &&
           gameState.CurrentPlayer is { } currentPlayer)
        {
            GameEngine gameEngine = new(gameState, currentPlayer);
            if (gameEngine.TriggerSpace(space))
            {
                if (gameState.GameStatus == GameStatus.GameOver && DetonateMines is {})
                {
                    DetonateMines();
                }
                NotifyStateChanged();
            }
            else
            {
                gameEngine.NextPlayersTurn();
            }
        }
    }
}
