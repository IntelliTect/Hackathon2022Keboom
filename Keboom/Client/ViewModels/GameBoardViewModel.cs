using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Keboom.Shared;
using Microsoft.AspNetCore.Components;

namespace Keboom.Client.ViewModels;

public class GameBoardViewModel : ViewModelBase
{
    public GameState? GameState { get; private set; }
    public Player ClientPlayer { get; set; }

    public IGameHubClientSideMethods GameEvents { get; }
    public IGameHubServerSideMethods ServerSideMethods { get; }
    public HttpClient HttpClient { get; }
    public NavigationManager Navigation { get; }

    public GameBoardViewModel(IGameHubClientSideMethods hubEvents, IGameHubServerSideMethods hubMethods, HttpClient httpClient, NavigationManager navigation)
    {
        GameEvents = hubEvents;
        ServerSideMethods = hubMethods;
        HttpClient = httpClient;
        Navigation = navigation;
    }

    public override async Task OnInitializedAsync()
    {
        GameEvents.GameStateUpdated -= OnGameStateUpdated;
        GameEvents.GameStateUpdated += OnGameStateUpdated;

        Navigation.TryGetQueryString("gameName", out string? gameName);
        Navigation.TryGetQueryString("playerName", out string? playerName);

        if (gameName is null)
        {
             gameName = Guid.NewGuid().ToString().Substring(0, 4);
        }

        if (playerName is null)
        {
            playerName = "foo";
        }

        ClientPlayer = new Player()
        {
            Name = playerName,
            Id = Guid.NewGuid().ToString()
        };

        var joinGameRequest = new JoinGameRequest
        {
            GameName = gameName,
            BoardWidth = 8,
            BoardHeight = 8,
            NumberOfMines = 15,
            PlayerId = ClientPlayer.Id,
            PlayerName = playerName
        };

        using var response = await HttpClient.PostAsJsonAsync("/game", joinGameRequest);

        GameState = await response.Content.ReadFromJsonAsync<GameState>();

        //var joinGameRequest2 = new JoinGameRequest
        //{
        //    GameName = gameName,
        //    BoardWidth = 8,
        //    BoardHeight = 8,
        //    NumberOfMines = 15,
        //    PlayerId = Guid.NewGuid().ToString(),
        //    PlayerName = $"Player 2"
        //};

        //using var response2 = await HttpClient.PostAsJsonAsync("/game", joinGameRequest2);

        //GameState = await response2.Content.ReadFromJsonAsync<GameState>();

        await ServerSideMethods.JoinGame(gameName);

        await base.OnInitializedAsync();
    }

    private void OnGameStateUpdated(object? sender, EventArgs<GameState> e)
    {
        Console.WriteLine("Got new game state");
        GameState = e.Value;
        NotifyStateChanged();
    }
}
