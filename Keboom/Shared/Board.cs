﻿
namespace Keboom.Shared;

public class Board
{
    public BoardSpace this[int x, int y]
    {
        get => FlatGrid[x * Width + y];
        set => FlatGrid[x * Width + y] = value;
    }

    public BoardSpace[] FlatGrid { get; set; }

    public int Width { get; }
    public int Height { get; }
    public int NumberOfMines { get; }

    public Board(int width, int height, int numberOfMines)
    {
        FlatGrid = new BoardSpace[width * height];
        Width = width;
        Height = height;
        NumberOfMines = numberOfMines;
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                FlatGrid[x * width + y] = new BoardSpace(x, y);
            }
        }
        SetMines(NumberOfMines);
        SetAdjacentCounts();
    }

    private void SetMines(int numberOfMines)
    {
        Random random = new();

        for (int mine = 0; mine < numberOfMines; mine++)
        {
            while (true)
            {
                var x = random.Next(Width);
                var y = random.Next(Height);

                if (!this[x, y].HasMine)
                {
                    this[x, y].HasMine = true;
                    break;
                }
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
        => GetAdjacent(x, y).Count(x => x.HasMine);

    public void SetAdjacentCounts()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                this[x, y].AdjacentMines = GetAdjacentCount(x, y);
            }
        }
    }
}
