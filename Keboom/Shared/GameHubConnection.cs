using Microsoft.AspNetCore.SignalR.Client;
using Polly;

namespace Keboom.Shared;

public class GameHubConnection : IGameHubServerSideMethods
{
    HubConnection HubConnection { get; set; }
    string HubUrl { get; set; }
    public IGameHubClientSideMethods Handler { get; }
    public IGameHubEventHandler HubEventHandler { get; }

    public GameHubConnection(string url, IGameHubClientSideMethods handler, IGameHubEventHandler gameHubEventHandler){
        Handler = handler;
        HubEventHandler = gameHubEventHandler;
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

        SetHandler(handler);

        HubConnection.Closed += async (error) =>
        {
            HubEventHandler.LostConnection();
            Console.Error.WriteLine($"Connection lost to hub: {error?.Message}");
            await Open();
        };

        Task.Run(() => Open());
    }

    private async Task Open()
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(20);
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(
                i => pauseBetweenFailures,
                (exception, timeSpan) =>
                {
                    HubEventHandler.LostConnection();
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
            HubEventHandler.Connected();
            Console.WriteLine("SignalR connection established");
            return true;
        }
    }

    private void SetHandler(IGameHubClientSideMethods gameHubClientSideMethods)
    {
        HubConnection.On<GameState> (
            nameof(IGameHubClientSideMethods.StartGame),
            gameHubClientSideMethods.StartGame
        );

        HubConnection.On<string>(
            nameof(IGameHubClientSideMethods.NewGameId),
            gameHubClientSideMethods.NewGameId
        );

        HubConnection.On<string>(
            nameof(IGameHubClientSideMethods.PlayerLeft),
            gameHubClientSideMethods.PlayerLeft
        );

        HubConnection.On<GameState>(
            nameof(IGameHubClientSideMethods.GameState),
            gameHubClientSideMethods.GameState
        );
    }

    public Task GameState(GameState gameState) => HubConnection.InvokeAsync(nameof(IGameHubServerSideMethods.GameState), gameState);
    public Task LeaveGame() => HubConnection.InvokeAsync(nameof(IGameHubServerSideMethods.LeaveGame));
    public Task JoinGame(string gameId, string playername) => HubConnection.InvokeAsync(nameof(IGameHubServerSideMethods.JoinGame), gameId, playername);
    public Task CreateGame(string playerName) => HubConnection.InvokeAsync(nameof(IGameHubServerSideMethods.CreateGame), playerName);
}
