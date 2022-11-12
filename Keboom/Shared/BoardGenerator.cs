﻿namespace Keboom.Shared;

public class BoardGenerator
{
    public Board CreateBoard(int width, int height, int numberOfMines)
    {
        var board = new Board(width, height);

        SetMines(board, numberOfMines);
        SetAdjacentCounts(board);

        return board;
    }

    private void SetMines(Board board, int numberOfMines)
    {
        var random = new Random();

        for (int mine = 0; mine < numberOfMines; mine++)
        {
            while (true)
            {
                var x = random.Next(board.Width);
                var y = random.Next(board.Height);

                if (!board.Grid[x, y].HasMine)
                {
                    board.Grid[x, y].HasMine = true;
                    break;
                }
            }
        }
    }

    private void SetAdjacentCounts(Board board)
    {
        for(var x = 0; x < board.Width; x++)
        {
            for(var y = 0; y < board.Height; y++)
            {
                board.Grid[x, y].AdjacentMines = board.GetAdjacentCount(x, y);
            }
        }
    }
}
