namespace Keboom.Shared;

public class Board
{
    public BoardSpace[,] Grid { get; init; }

    public Board(int width, int height)
    {
        Grid = new BoardSpace[width, height];
    }
}
