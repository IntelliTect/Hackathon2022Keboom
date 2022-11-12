using Keboom.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Keboom.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;

    public GameController(ILogger<GameController> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    [Route(nameof(CreateNewGame))]
    public GameState CreateNewGame(string player1Name, string player2Name)
    {
        var newGame =  new GameState()
        {
            Id = Guid.NewGuid().ToString(),
            Board = BoardGenerator.CreateBoard(4, 4, 5),
            Player1 = new Player() { Id = Guid.NewGuid().ToString(), Name=player1Name },
            Player2 = new Player() { Id = Guid.NewGuid().ToString(), Name = player2Name }
        };

        return newGame;
    }

}
