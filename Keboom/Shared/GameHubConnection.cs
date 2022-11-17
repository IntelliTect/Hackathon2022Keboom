using Microsoft.AspNetCore.SignalR.Client;
using Polly;

namespace Keboom.Shared;

public class GameHubConnection : IGameHubServerSideMethods, IGameHubClientSideMethods, IGameHubEventHandler
{
    private HubConnection HubConnection { get; }
    private string HubUrl { get; }

    public GameHubConnection(string url)
    {
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

        RegisterEvents();

        HubConnection.Closed += async (error) =>
        {
            ConnectionLost?.Invoke(this, EventArgs.Empty);
            Console.Error.WriteLine($"Connection lost to hub: {error?.Message}");
            await Open();
        };
    }

    public event EventHandler? ConnectionLost;
    public event EventHandler? Connected;
    public event EventHandler<EventArgs>? PlayerLeft;
    public event EventHandler<EventArgs<GameState>>? GameStateUpdated;

    public async Task Open()
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

    private void RegisterEvents()
    {
        HubConnection.On(
            nameof(IGameHubClientSideMethods.PlayerLeft),
            () => PlayerLeft?.Invoke(this, new EventArgs())
        );

        HubConnection.On<GameState>(
            nameof(IGameHubClientSideMethods.GameStateUpdated),
            (gameState) => GameStateUpdated?.Invoke(this, new EventArgs<GameState>(gameState))
        );
    }

    public Task GameState(GameState gameState) => HubConnection.InvokeAsync(nameof(IGameHubServerSideMethods.GameState), gameState);
    public Task LeaveGame() => HubConnection.InvokeAsync(nameof(IGameHubServerSideMethods.LeaveGame));
    public Task JoinGame(string gameId, string playerId) => HubConnection.InvokeAsync(nameof(IGameHubServerSideMethods.JoinGame), gameId, playerId);
}
