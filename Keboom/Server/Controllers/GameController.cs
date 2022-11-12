using System.Reflection.Metadata.Ecma335;
using Keboom.Server.Hubs;
using Keboom.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Keboom.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly IGameStore gameStore;
    private readonly IHubContext<GameHub> gameHub;

    public GameController(ILogger<GameController> logger, IGameStore gameStore, IHubContext<GameHub> gameHub)
    {
        _logger = logger;
        this.gameStore = gameStore;
        this.gameHub = gameHub;
    }

    [HttpPost]
    public async Task<GameState> JoinGame([FromBody] JoinGameRequest joinGameRequest)
    {
        var gameState = gameStore.JoinGame(joinGameRequest);

        await gameHub.Clients
            .Group(gameState.Id)
            .SendAsync(nameof(IGameHubClientSideMethods.GameStateUpdated), gameState);

        return gameState;
    }

}
