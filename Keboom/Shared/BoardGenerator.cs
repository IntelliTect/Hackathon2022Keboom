using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keboom.Shared;

public class BoardGenerator
{
    public Board CreateBoard(int width, int height, int numberOfMines)
    {
        var board = new Board(width, height);

        var random = new Random();

        for(int mine = 0; mine < numberOfMines; mine++)
        {
            while (true)
            {
                var x = random.Next(width);
                var y = random.Next(height);

                if (!board.Grid[x, y].HasMine)
                {
                    board.Grid[x, y].HasMine = true;
                    break;
                }
            }
        }

        return board;
    }
}
