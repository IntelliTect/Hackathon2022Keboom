namespace Keboom.Shared;

public class GameState
{
    public string Id { get; set; } = default!;
    public Board? Board { get; set; }
    public Player? Player1 { get; set; }
    public Player? Player2 { get; set; }

    public List<Player?> Players => new(){ Player1, Player2 };

    public Player? CurrentPlayer { get; set; }

    public void NextPlayersTurn()
    {
        int index = Players.IndexOf(CurrentPlayer);
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
}
