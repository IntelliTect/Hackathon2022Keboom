﻿namespace Keboom.Shared;

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

        if (boardSpace.AdjacentMines == 0)
        {
            foreach (var adjacentSpace in GameState.Board!.GetAdjacent(boardSpace.X, boardSpace.Y))
            {
                TriggerSpace(adjacentSpace);
            }
        }

        return false;
    }

    private void OpenBlanks(BoardSpace boardSpace)
    {
        boardSpace.ClaimedByPlayer = Player.Id;

        foreach (var adjacentSpace in GameState.Board!.GetAdjacent(boardSpace.X, boardSpace.Y))
        {
            if (adjacentSpace.ClaimedByPlayer is null && adjacentSpace.AdjacentMines == 0 && !adjacentSpace.HasMine)
            {
                OpenBlanks(adjacentSpace);
            }
        }
    }
}
