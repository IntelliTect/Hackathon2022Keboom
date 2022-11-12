﻿namespace Keboom.Shared;

public class GameState
{
    public Guid Id { get; set; }
    public Board? Board { get; set; }
    public Player? Player1 { get; set; }
    public Player? Player2 { get; set; }

    public Player?[] Players => new Player?[2] { Player1, Player2 };

    public Player? CurrentPlayer { get; set; }
}