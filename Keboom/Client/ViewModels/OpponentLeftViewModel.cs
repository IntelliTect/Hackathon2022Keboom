using Microsoft.AspNetCore.Components;

namespace Keboom.Client.ViewModels;

public partial class OpponentLeftViewModel : ViewModelBase
{

    public NavigationManager Navigation { get; }

    [ObservableProperty]
    private string? _PlayerName;

    public OpponentLeftViewModel(NavigationManager navigation)
    {

        Navigation = navigation;
    }

    public override async Task OnInitializedAsync()
    {
        Navigation.TryGetQueryString("playerName", out string? playerName);

        PlayerName = playerName;


        await base.OnInitializedAsync();
    }



    public void GoToHome()
    {
        if (PlayerName != null)
        {
            Navigation.NavigateTo(Navigation.GetUriWithQueryParameters("/", new Dictionary<string, object?>
        {
            {"playerName", PlayerName },
        }));
        }
        else
        {
            Navigation.NavigateTo("/");

        }
    }


}
