namespace Keboom.Shared;

public class Board
{
    public BoardSpace[,] Grid { get; }
    public int Width { get; }
    public int Height { get; }

    public Board(int width, int height)
    {
        Grid = new BoardSpace[width, height];
        Width = width;
        Height = height;

        for(var x = 0; x < width; x++)
        {
            for(var y = 0; y < height; y++)
            {
                Grid[x, y] = new BoardSpace();
            }
        }
    }

    public int GetAdjacentCount(int x, int y)
    {
        var count = 0;

        if (x > 0)
        {
            count += Grid[x - 1, y].HasMine ? 1 : 0;
        }
        if (x < Width - 1)
        {
            count += Grid[x + 1, y].HasMine ? 1 : 0;
        }

        if (y > 0)
        {
            count += Grid[x, y - 1].HasMine ? 1 : 0;
        }
        if (y < Height - 1)
        {
            count += Grid[x, y + 1].HasMine ? 1 : 0;
        }

        if (x > 0 && y > 0)
        {
            count += Grid[x - 1, y - 1].HasMine ? 1 : 0;
        }
        if (x > 0 && y < Height - 1)
        {
            count += Grid[x - 1, y + 1].HasMine ? 1 : 0;
        }

        if (x < Width - 1 && y > 0)
        {
            count += Grid[x + 1, y - 1].HasMine ? 1 : 0;
        }
        if (x < Width - 1 && y < Height - 1)
        {
            count += Grid[x + 1, y + 1].HasMine ? 1 : 0;
        }

        return count;
    }
}
