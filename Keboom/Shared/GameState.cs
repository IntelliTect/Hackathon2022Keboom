namespace Keboom.Shared;

public class GameState
{
    private Player? currentPlayer;
    public string Id { get; set; } = "";
    public Board? Board { get; set; }
    public Player? Player1 => Players.FirstOrDefault();
    public Player? Player2 => Players.Skip(1).FirstOrDefault();
    public GameStatus GameStatus { get; set; }

    public List<Player> Players { get; init; } = new();

    public Player? CurrentPlayer
    {
        get
        {
            return Players.FirstOrDefault(p => p.Id == CurrentPlayerId);
        }
        set
        {
            CurrentPlayerId = value?.Id;
        }
    }

    public string? CurrentPlayerId { get; set; }

    public Player GetPlayer(string playerId)
        => Players.FirstOrDefault(p => p.Id == playerId)
            ?? throw new ArgumentException($"Player with id {playerId} not found");
}
