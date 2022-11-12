namespace Keboom.Client.ViewModels;

public class GameBoardViewModel : ViewModelBase
{

    public GameState? GameState { get; private set; }
    public IGameHubClientSideMethods GameEvents { get; }
    public IGameHubServerSideMethods ServerSideMethods { get; }

    public GameBoardViewModel(IGameHubClientSideMethods hubEvents, IGameHubServerSideMethods hubMethods)
    {
        GameEvents = hubEvents;
        ServerSideMethods = hubMethods;
    }

    public override Task OnInitializedAsync()
    {
        GameEvents.NewGameId -= OnNewGameId;
        GameEvents.NewGameId += OnNewGameId;

        GameEvents.GameStarted -= OnGameStart;
        GameEvents.GameStarted += OnGameStart;

        GameEvents.GameStateUpdated -= OnGameStateUpdated;
        GameEvents.GameStateUpdated += OnGameStateUpdated;

        GameState = new GameState
        {
            Board = new (8, 8, 15),
            GameStatus = GameStatus.InProgress,
            Player1 = new Player
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Player 1",
                Score = 0,
                Color = PlayerColor.Red
            },
            Player2 = new Player
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Player 2",
                Score = 0,
                Color = PlayerColor.Yellow
            }
        };


        ServerSideMethods.CreateGame("Inigo");

        GameState.CurrentPlayer = GameState.Player1;
        return base.OnInitializedAsync();
    }

    private void OnGameStateUpdated(object? sender, EventArgs<GameState> e) => throw new NotImplementedException();
    private void OnGameStart(object? sender, EventArgs<GameState> e) => throw new NotImplementedException();
    private void OnNewGameId(object? sender, EventArgs<string> e) => Console.WriteLine($"new game! id: {e}");

}
