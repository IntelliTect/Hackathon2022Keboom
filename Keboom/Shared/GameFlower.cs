using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keboom.Shared;

public class GameFlower
{
    public GameState GameState { get; }
    public Player Player { get; }

    public GameFlower(GameState gameState, Player player)
    {
        GameState = gameState;
        Player = player;
    }

    public bool TriggerSpace(BoardSpace boardSpace)
    {
        if (boardSpace.ClaimedByPlayer is not null)
        {
            return true;
        }

        boardSpace.ClaimedByPlayer = Player.Id;

        if (boardSpace.HasMine)
        {
            Player.Score++;
            return true;
        }

        return false;
    }
}
