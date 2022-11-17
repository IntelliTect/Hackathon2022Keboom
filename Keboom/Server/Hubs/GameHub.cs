using Keboom.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Keboom.Server.Hubs;


public class GameHub : Hub, IGameHubServerSideMethods
{
    private readonly IGameStore gameStore;
    private readonly ILogger<GameHub> logger;

    public GameHub(
        IGameStore gameStore,
        ILogger<GameHub> logger
    ) : base()
    {
        this.gameStore = gameStore;
        this.logger = logger;
    }

    public override async Task<Task> OnDisconnectedAsync(Exception? exception)
    {
        var playerId = Context.ConnectionId;

        string? game = gameStore.GetGame(playerId);
        gameStore.RemoveFromGame(playerId);

        if (game is not null)
        {
            await Clients.Group(game).SendAsync(nameof(IGameHubClientSideMethods.PlayerLeft));
        }

        return base.OnDisconnectedAsync(exception);
    }

    [HubMethodName(nameof(JoinGame))]
    public async Task JoinGame(string gameId, string playerId)
    {
        gameStore.AddPlayerConnectionID(gameId, playerId, Context.ConnectionId);
        await Groups.AddToGroupAsync(
                            Context.ConnectionId,
                            gameId
                        );
    }


    [HubMethodName(nameof(LeaveGame))]
    public async Task LeaveGame()
    {
        var gameId = gameStore.GetGame(Context.ConnectionId);

        if (gameId is null)
        {
            return;
        }
        await Groups.RemoveFromGroupAsync(
                            Context.ConnectionId,
                            gameId
                        );

        await Clients.Group(gameId)
            .SendAsync(nameof(IGameHubClientSideMethods.PlayerLeft));

        gameStore.RemoveFromGame(Context.ConnectionId);

    }

    [HubMethodName(nameof(GameState))]
    public async Task GameState(GameState gameState)
    {
        await Clients
            .Group(gameState.Id)
            .SendAsync(nameof(IGameHubClientSideMethods.GameStateUpdated), gameState);
    }

}


