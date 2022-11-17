namespace Keboom.Client.ViewModels;

public partial class MinefieldViewModel : ViewModelBase
{
    public MinefieldViewModel(IGameHubServerSideMethods hubMethods)
    {
        HubMethods = hubMethods;
    }

    [ObservableProperty]
    private GameState? _gameState;

    public Action? OnGameOver { get; set; }

    public BoardSpace? MineOpenedByPlayer { get; set; }

    public IGameHubServerSideMethods HubMethods { get; }

    [RelayCommand]
    private void OpenCell(BoardSpace space)
    {
        MineOpenedByPlayer = null;
        if (GameState is { } gameState &&
           gameState.CurrentPlayer is { } currentPlayer)
        {
            GameEngine gameEngine = new(gameState, currentPlayer);
            if (gameEngine.TriggerSpace(space))
            {
                MineOpenedByPlayer = space;
                if (gameState.GameStatus == GameStatus.GameOver)
                {
                    OnGameOver?.Invoke();
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
        if (value?.GameStatus == GameStatus.GameOver)
        {
            OnGameOver?.Invoke();
        }
    }
}
