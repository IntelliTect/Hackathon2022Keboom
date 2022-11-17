using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Keboom.Client.ViewModels;

public partial class GameBoardViewModel : ViewModelBase
{
    public GameState? GameState { get; private set; }
    public Player? ClientPlayer { get; set; }

    public IGameHubClientSideMethods GameEvents { get; }
    public IGameHubServerSideMethods ServerSideMethods { get; }
    public HttpClient HttpClient { get; }
    public NavigationManager Navigation { get; }
    public IJSRuntime JSRuntime { get; }

    [ObservableProperty]
    private string? _GameInviteUrl;

    public GameBoardViewModel(
        IGameHubClientSideMethods hubEvents,
        IGameHubServerSideMethods hubMethods,
        HttpClient httpClient,
        NavigationManager navigation,
        IJSRuntime jSRuntime)
    {
        GameEvents = hubEvents;
        ServerSideMethods = hubMethods;
        HttpClient = httpClient;
        Navigation = navigation;
        JSRuntime = jSRuntime;
    }

    public override async Task OnInitializedAsync()
    {
        GameEvents.GameStateUpdated -= OnGameStateUpdated;
        GameEvents.GameStateUpdated += OnGameStateUpdated;

        Navigation.TryGetQueryString("gameName", out string? gameName);
        Navigation.TryGetQueryString("playerName", out string? playerName);
        Navigation.TryGetQueryString("isPublic", out bool isPublic);

        var isPublicString = isPublic ? "public" : "private";
        Console.WriteLine($"Creating {isPublicString} game");

        if (gameName is null)
        {
            gameName = Guid.NewGuid().ToString().Substring(0, 4);
        }

        if (playerName is null)
        {
            playerName = "Unknown";
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
            PlayerName = playerName,
            IsPublic = isPublic,
        };

        using var response = await HttpClient.PostAsJsonAsync("/game", joinGameRequest);

        GameState = await response.Content.ReadFromJsonAsync<GameState>();

        if (GameState is not null)
        {
            await ServerSideMethods.JoinGame(GameState.Id);

            GameInviteUrl = $"{Navigation.BaseUri}?gamename={GameState.Id}";
        }

        await base.OnInitializedAsync();
    }

    private void OnGameStateUpdated(object? sender, EventArgs<GameState> e)
    {
        GameState = e.Value;
        NotifyStateChanged();
    }

    public async Task CopyGameLinkToClipboard()
    {
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", GameInviteUrl);
    }
}
