
namespace Keboom.Shared;

public class Board
{
    public BoardSpace this[int x, int y]
    {
        get { return FlatGrid[x * Width + y]; }
        set { FlatGrid[x * Width + y] = value; }
    }

    public BoardSpace[] FlatGrid { get; set; }

    public int Width { get; }
    public int Height { get; }

    public Board(int width, int height)
    {
        FlatGrid = new BoardSpace[width * height];
        Width = width;
        Height = height;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                FlatGrid[x * width + y] = new BoardSpace(x, y);
            }
        }
    }

    public List<BoardSpace> GetAdjacent(int x, int y)
    {
        var list = new List<BoardSpace>();

        if (x > 0)
        {
            list.Add(this[x - 1, y]);
        }
        if (x < Width - 1)
        {
            list.Add(this[x + 1, y]);
        }

        if (y > 0)
        {
            list.Add(this[x, y - 1]);
        }
        if (y < Height - 1)
        {
            list.Add(this[x, y + 1]);
        }

        if (x > 0 && y > 0)
        {
            list.Add(this[x - 1, y - 1]);
        }
        if (x > 0 && y < Height - 1)
        {
            list.Add(this[x - 1, y + 1]);
        }

        if (x < Width - 1 && y > 0)
        {
            list.Add(this[x + 1, y - 1]);
        }
        if (x < Width - 1 && y < Height - 1)
        {
            list.Add(this[x + 1, y + 1]);
        }

        return list;
    }

    public int GetAdjacentCount(int x, int y)
    {
        return GetAdjacent(x, y).Count(x => x.HasMine);
    }
}
