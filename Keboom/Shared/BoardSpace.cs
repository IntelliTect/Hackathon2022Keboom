namespace Keboom.Shared;

public class BoardSpace
{
    public bool HasMine { get; set; }
    public int AdjacentMines { get; set; }

    public int? ClaimedByPlayer { get; set; }
}
