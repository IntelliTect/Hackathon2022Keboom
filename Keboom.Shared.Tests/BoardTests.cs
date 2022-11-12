using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keboom.Shared.Tests;

public class BoardTests
{
    [Fact]
    public void GetAdjacentCount_NoAdjacent_0()
    {
        var board = new Board(3, 3);

        var hasMine = board.GetAdjacentCount(1, 1);

        Assert.Equal(0, hasMine);
    }

    [Fact]
    public void GetAdjacentCount_4Adjacent_4()
    {
        var board = new Board(3, 3);
        board[0, 0].HasMine = true;
        board[1, 0].HasMine = true;
        board[2, 0].HasMine = true;
        board[0, 1].HasMine = true;

        var hasMine = board.GetAdjacentCount(1, 1);

        Assert.Equal(4, hasMine);
    }

    [Fact]
    public void GetAdjacentCount_3Corner_3()
    {
        var board = new Board(3, 3);
        board[1, 0].HasMine = true;
        board[1, 1].HasMine = true;
        board[0, 1].HasMine = true;

        var hasMine = board.GetAdjacentCount(0, 0);

        Assert.Equal(3, hasMine);
    }
}
