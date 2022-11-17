using System.Xml.Linq;
using Microsoft.AspNetCore.Components;

namespace Keboom.Client.ViewModels;

public partial class GameOverViewModel : ViewModelBase
{
    public GameOverViewModel(NavigationManager navigationManager)
    {
        Navigation = navigationManager;
    }

    private NavigationManager Navigation { get; }

    public string? CurrentPlayerName { get; set; }

    [RelayCommand]
    private void NewGame()
    {
        Navigation.NavigateTo($"/GameBoard?playerName={CurrentPlayerName}");
    }

    [RelayCommand]
    private void QuitGame()
    {
        Navigation.NavigateTo("/AboutUs");
    }
}
