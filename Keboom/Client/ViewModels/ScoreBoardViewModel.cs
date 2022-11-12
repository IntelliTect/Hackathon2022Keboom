namespace Keboom.Client.ViewModels;

public partial class ScoreBoardViewModel : ViewModelBase
{
    [ObservableProperty]
    private GameState? _gameState;

    partial void OnGameStateChanging(GameState? value)
    {
        if (_gameState is { } oldGameState)
        {
            oldGameState.CurrentPlayerChanged -= CurrentPlayerChanged;
            oldGameState.Players.ForEach(p =>
            {
                p.ScoreChanged -= ScoreChanged;
            });
        }
        if (value is not null)
        {
            value.CurrentPlayerChanged += CurrentPlayerChanged;

            value.Players.ForEach(p =>
            {
                p.ScoreChanged -= ScoreChanged;
                p.ScoreChanged += ScoreChanged;
            });
        }
    }

    private void CurrentPlayerChanged(object? sender, EventArgs e)
    {
        //TODO Not sure the best way to have the VM trigger a render
        OnPropertyChanged(nameof(GameState));
    }

    private void ScoreChanged(object? sender, EventArgs e)
    {
        //TODO Not sure the best way to have the VM trigger a render
        OnPropertyChanged(nameof(GameState));
    }

    public bool GameHasPlayers => GameState is { } gameState &&
        gameState.Players.Any() &&
        gameState.Players.All(x => x is not null);
}
