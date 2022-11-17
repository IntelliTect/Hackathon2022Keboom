namespace Keboom.Shared;

public class GameEngine
{
    public GameState GameState { get; }
    public Player Player { get; }

    public GameEngine(GameState gameState, Player player)
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
            UpdateGameStatus();
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

    private void UpdateGameStatus()
    {
        if (Player.Score > Math.Floor(GameState.Board?.NumberOfMines/2.0 ?? int.MaxValue))
        {
            GameState.GameStatus = GameStatus.GameOver;
        }
    }

    public void NextPlayersTurn()
    {
        int index = GameState.CurrentPlayer is { } current ? GameState.Players.IndexOf(current) : -1;
        if (index >= 0)
        {
            index = (index + 1) % GameState.Players.Count;
        }
        else
        {
            index = 0;
        }
        GameState.CurrentPlayer = GameState.Players[index];
    }

}
