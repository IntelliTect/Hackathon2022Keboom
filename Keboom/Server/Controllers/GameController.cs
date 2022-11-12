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
        return new GameState()
        {
            Id = Guid.NewGuid(),
            Board = BoardGenerator.CreateBoard(4, 4, 5),
            Player1 = new Player() { Id = Guid.NewGuid(), Name=player1Name },
            Player2 = new Player() { Id = Guid.NewGuid(), Name = player2Name }
        };
    }

}
