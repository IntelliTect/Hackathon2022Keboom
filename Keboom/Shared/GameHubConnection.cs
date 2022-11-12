using Microsoft.AspNetCore.SignalR.Client;
using Polly;

namespace Keboom.Shared;

public class GameHubConnection : IGameHubServerSideMethods, IGameHubClientSideMethods, IGameHubEventHandler
{
    HubConnection HubConnection { get; set; }
    string HubUrl { get; set; }
    
    public GameHubConnection(string url){
       
        HubUrl = url;
    
     
        HubConnection = new HubConnectionBuilder()
            .WithUrl(
                HubUrl,
                options =>
                {
                    //options.Headers.Add("SrAuthorization", $"Bearer {gatewayId}");
                    //options.Headers.Add("ClientType", SRHubClientType.Gateway);
                }
            )
           /* .AddJsonProtocol(
                options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy =
                        System.Text.Json.JsonNamingPolicy.CamelCase;
                    options.PayloadSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                }
            )*/
            .Build();

        SetEvents();

        HubConnection.Closed += async (error) =>
        {
            ConnectionLost?.Invoke(this,EventArgs.Empty);
            Console.Error.WriteLine($"Connection lost to hub: {error?.Message}");
            await Open();
        };

        Task.Run(() => Open());
    }

    public event EventHandler? ConnectionLost;
    public event EventHandler? Connected;
    public event EventHandler<EventArgs<string>>? PlayerLeft;
    public event EventHandler<EventArgs<string>>? NewGameId;
    public event EventHandler<EventArgs<GameState>>? GameStarted;
    public event EventHandler<EventArgs<GameState>>? GameStateUpdated;

    private async Task Open()
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(20);
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(
                i => pauseBetweenFailures,
                (exception, timeSpan) =>
                {
                    ConnectionLost?.Invoke(this, EventArgs.Empty);
                    Console.Error.WriteLine(
                        $"Error connecting to SignalR hub {HubUrl} - {exception.Message}"
                    );
                }
            );
        await retryPolicy.ExecuteAsync(
            async () =>
            {
                await TryOpen();
            }
        );
        async Task<bool> TryOpen()
        {
            Console.WriteLine("Starting SignalR connection");
            await HubConnection.StartAsync();
            Connected?.Invoke(this, EventArgs.Empty);
            Console.WriteLine("SignalR connection established");
            return true;
        }
    }

    private void SetEvents()
    {
        HubConnection.On<GameState> (
            nameof(IGameHubClientSideMethods.GameStarted),
            (gameState)=>GameStarted?.Invoke(this, new EventArgs<GameState>(gameState))
        );

        HubConnection.On<string>(
            nameof(IGameHubClientSideMethods.NewGameId),
            (gameId) => NewGameId?.Invoke(this, new EventArgs<string>(gameId))
        );

        HubConnection.On<string>(
            nameof(IGameHubClientSideMethods.PlayerLeft),
            (playerId) => PlayerLeft?.Invoke(this, new EventArgs<string>(playerId))
        );

        HubConnection.On<GameState>(
            nameof(IGameHubClientSideMethods.GameStateUpdated),
            (gameState) => GameStateUpdated?.Invoke(this, new EventArgs<GameState>(gameState))
        );
    }

    public Task GameState(GameState gameState) => HubConnection.InvokeAsync(nameof(IGameHubServerSideMethods.GameState), gameState);
    public Task LeaveGame() => HubConnection.InvokeAsync(nameof(IGameHubServerSideMethods.LeaveGame));
    public Task JoinGame(string gameId, string playername) => HubConnection.InvokeAsync(nameof(IGameHubServerSideMethods.JoinGame), gameId, playername);
    public Task CreateGame(string playerName) => HubConnection.InvokeAsync(nameof(IGameHubServerSideMethods.CreateGame), playerName);
}
