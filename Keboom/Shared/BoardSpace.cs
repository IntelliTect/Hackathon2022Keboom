namespace Keboom.Shared;

public class BoardSpace
{
    public bool HasMine { get; set; }
    public int AdjacentMines { get; set; }

    public string? ClaimedByPlayer { get; set; }
    public int X { get; }
    public int Y { get; }

    public BoardSpace(int x , int y)
    {
        X = x;
        Y = y;
    }
}
