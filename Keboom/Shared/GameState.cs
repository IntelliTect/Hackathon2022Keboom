namespace Keboom.Shared;

public class GameState
{
    public string Id { get; set; } = default!;
    public Board? Board { get; set; }
    public Player? Player1 { get; set; }
    public Player? Player2 { get; set; }

    public List<Player> Players => EnumeratePlayers().ToList();

    public Player? CurrentPlayer { get; set; }

    //TODO This should move to GameFlower
    public void NextPlayersTurn()
    {
        int index = CurrentPlayer is { } current ? Players.IndexOf(current) : -1;
        if (index >= 0)
        {
            index = (index + 1) % Players.Count;
        }
        else
        {
            index = 0;
        }
        CurrentPlayer = Players[index];
    }

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
