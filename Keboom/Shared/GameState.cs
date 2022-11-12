namespace Keboom.Shared;

public class GameState
{
    private Player? currentPlayer;

    public event EventHandler? CurrentPlayerChanged;
    public string Id { get; set; } = "";
    public Board? Board { get; set; }
    public Player? Player1 => Players.FirstOrDefault();
    public Player? Player2 => Players.Skip(1).FirstOrDefault();
    public GameStatus GameStatus { get; set; }

    public List<Player> Players { get; } = new();

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
}
