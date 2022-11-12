namespace Keboom.Client.ViewModels;

public partial class ScoreBoardViewModel : ViewModelBase
{
    [ObservableProperty]
    private GameState? _gameState;

    partial void OnGameStateChanging(GameState? value)
    {
        NotifyStateChanged();
    }

    public bool GameHasPlayers => GameState is { } gameState &&
        gameState.Players.Any() &&
        gameState.Players.All(x => x is not null);
}
