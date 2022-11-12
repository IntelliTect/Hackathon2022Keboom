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
    private string? _CreatorPlayerName;

    [ObservableProperty]
    private string? _JoiningPlayerName;

    public IndexViewModel( HttpClient httpClient, NavigationManager navigation)
    {
      
        HttpClient = httpClient;
        Navigation = navigation;
    }

    public override async Task OnInitializedAsync()
    {
      

        Navigation.TryGetQueryString("gameName", out string? gameName);
        GameName = gameName;

       // Navigation.
      
        await base.OnInitializedAsync();
    }



    private void NavigateToGameBoardAndJoinGame()
    {
        Navigation.NavigateTo($"gameboard?playerName={JoiningPlayerName}&{nameof(GameName)}={GameName}");
    }

    private void NavigateToGameBoardAndCreateGame()
    {
        Navigation.NavigateTo($"gameboard?playerName={CreatorPlayerName}");
    }


}
