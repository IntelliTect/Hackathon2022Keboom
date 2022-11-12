using Keboom.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Keboom.Server.Hubs;


public class GameHub : Hub , IGameHubServerSideMethods
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

    public override async Task<Task> OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override async Task<Task> OnDisconnectedAsync(Exception exception)
    {
        var playerId = Context.ConnectionId;

        string? game = gameStore.GetGame(playerId);
        gameStore.RemoveFromGame(playerId);

        if (game is not null) {
            await Clients.Group(game).SendAsync(nameof(IGameHubClientSideMethods.PlayerLeft), playerId);
        }

        return base.OnDisconnectedAsync(exception);
    }


    [HubMethodName(nameof(CreateGame))]
    public async Task CreateGame(string playerName)
    {
        string playerId = Context.ConnectionId;

        string newGameId = Guid.NewGuid().ToString();
        gameStore.AddToGame(playerId, newGameId, playerName);

        await Groups.AddToGroupAsync(
                            Context.ConnectionId,
                            newGameId
                        );

        await Clients.Client(playerId).SendAsync(nameof(IGameHubClientSideMethods.NewGameId), newGameId);
    }

    [HubMethodName(nameof(JoinGame))]
    public async Task JoinGame(string gameId, string playername)
    {
        string playerId = Context.ConnectionId;
        gameStore.AddToGame(playerId, gameId, playername);

        await Groups.AddToGroupAsync(
                            Context.ConnectionId,
                            gameId
                        );

        if (gameStore.GamePlayerCount(gameId) == 2 )
        {
            var players = gameStore.GetGamePlayers(gameId);

            var newGame = new GameState()
            {
                Id = gameId,
                Board = BoardGenerator.CreateBoard(4, 4, 5),
                Player1 = players[0],
                Player2 = players[1],
                CurrentPlayer= players[0],
                
            };

            await Clients.Group(gameId).SendAsync(nameof(IGameHubClientSideMethods.StartGame) , newGame);
        
        }

    }


    [HubMethodName(nameof(LeaveGame))]
    public async Task LeaveGame()
    {
        var gameId = gameStore.GetGame(Context.ConnectionId);

        if (gameId is null) {
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
            .OthersInGroup(gameState.Id)
            .SendAsync(nameof(GameState), gameState);
    }

}


