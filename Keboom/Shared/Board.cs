using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keboom.Shared
{
    public class Board
    {
        BoardSpace[,] Grid { get; init; }

        public Board(int width, int height)
        {
            Grid = new BoardSpace[width, height];
        }
    }
}
