using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Keboom.Shared;
using Microsoft.AspNetCore.Components;

namespace Keboom.Client.ViewModels;

public partial class IndexViewModel : ViewModelBase
{
    public HttpClient HttpClient { get; }
    public NavigationManager Navigation { get; }

    [ObservableProperty]
    private string? _GameName;

    [ObservableProperty]
    private string? _PlayerName;

    [ObservableProperty]
    private bool _CreateButtonVisible = false;

    [ObservableProperty]
    private bool _JoinButtonVisible = false;

    
    public IndexViewModel( HttpClient httpClient, NavigationManager navigation)
    {
      
        HttpClient = httpClient;
        Navigation = navigation;
    }

    public override async Task OnInitializedAsync()
    {
      

        Navigation.TryGetQueryString("gameName", out string? gameName);
        GameName = gameName;

        if (gameName is not null)
        {
            JoinButtonVisible = true;
        }
        else {
            CreateButtonVisible= true;
        }

       // Navigation.
      
        await base.OnInitializedAsync();
    }

    
    public void NavigateToGameBoardAndJoinGame()
    {
        Navigation.NavigateTo($"gameboard?playerName={PlayerName}&{nameof(GameName)}={GameName}");
    }

    public void NavigateToGameBoardAndCreateGame()
    {
        Navigation.NavigateTo($"gameboard?playerName={PlayerName}");
    }


}
