namespace Keboom.Shared;

public class GameState
{
    private Player? currentPlayer;

    public event EventHandler? CurrentPlayerChanged;
    public string Id { get; set; } = "";
    public Board? Board { get; set; }
    public Player? Player1 { get; set; }
    public Player? Player2 { get; set; }
    public GameStatus GameStatus { get; set; }

    public List<Player> Players => EnumeratePlayers().ToList();

    public Player? CurrentPlayer
    {
        get => currentPlayer;
        set
        {
            if (currentPlayer != value)
            {
                currentPlayer = value;
                CurrentPlayerChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public Player GetPlayer(string playerId)
        => Players.FirstOrDefault(p => p.Id == playerId)
            ?? throw new ArgumentException($"Player with id {playerId} not found");

    private IEnumerable<Player> EnumeratePlayers()
    {
        if (Player1 is { } player1)
        {
            yield return player1;
        }
        if (Player2 is { } player2)
        {
            yield return player2;
        }
    }
}
