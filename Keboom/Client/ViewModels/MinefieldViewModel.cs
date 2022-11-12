namespace Keboom.Client.ViewModels;

public partial class MinefieldViewModel : ViewModelBase
{
    public MinefieldViewModel(IGameHubServerSideMethods hubMethods) {
        HubMethods = hubMethods;
    }

    [ObservableProperty]
    private GameState? _gameState;

    public Action? DetonateMines { get; set; }

    public IGameHubServerSideMethods HubMethods { get; }

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

            HubMethods.GameState(gameState);
        }
    }

    partial void OnGameStateChanged(GameState? value)
    {
        if (value?.GameStatus == GameStatus.GameOver && DetonateMines is { })
        {
            DetonateMines();
        }
    }
}
