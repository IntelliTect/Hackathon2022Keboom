using System.Net.Http.Json;
using Keboom.Shared;
﻿namespace Keboom.Client.ViewModels;

public class GameBoardViewModel : ViewModelBase
{

    public GameState? GameState { get; private set; }
    public IGameHubClientSideMethods GameEvents { get; }
    public IGameHubServerSideMethods ServerSideMethods { get; }
    public HttpClient HttpClient { get; }

    public GameBoardViewModel(IGameHubClientSideMethods hubEvents, IGameHubServerSideMethods hubMethods, HttpClient httpClient)
    {
        GameEvents = hubEvents;
        ServerSideMethods = hubMethods;
        HttpClient = httpClient;
    }

    public override async Task OnInitializedAsync()
    {
        GameEvents.GameStateUpdated -= OnGameStateUpdated;
        GameEvents.GameStateUpdated += OnGameStateUpdated;

        var joinGameRequest = new JoinGameRequest
        {
            GameName = "Inigo",
            BoardWidth = 8,
            BoardHeight = 8,
            NumberOfMines = 15,
            PlayerId = Guid.NewGuid().ToString(),
            PlayerName = $"Player {Guid.NewGuid()}"
        };

        using var response = await HttpClient.PostAsJsonAsync("/game", joinGameRequest);

        var joinGameRequest2 = new JoinGameRequest
        {
            GameName = "Inigo",
            BoardWidth = 8,
            BoardHeight = 8,
            NumberOfMines = 15,
            PlayerId = Guid.NewGuid().ToString(),
            PlayerName = $"Player 2"
        };

        using var response2 = await HttpClient.PostAsJsonAsync("/game", joinGameRequest2);

        GameState? gameState = await response2.Content.ReadFromJsonAsync<GameState>();

        GameStateUpdated(gameState);

        await base.OnInitializedAsync();
    }

    private void GameStateUpdated(GameState newGameState)
    {
        GameState= newGameState;
    }

    private void OnGameStateUpdated(object? sender, EventArgs<GameState> e) => throw new NotImplementedException();
}
