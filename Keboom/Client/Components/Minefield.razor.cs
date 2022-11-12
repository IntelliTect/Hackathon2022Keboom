using Keboom.Client.Converters;
using Microsoft.AspNetCore.Components;

namespace Keboom.Client.Components;

public partial class Minefield
{
    [CascadingParameter(Name = "GameState")]
    public GameState? GameState
    {
        get => ViewModel.GameState;
        set => ViewModel.GameState = value;
    }

    [CascadingParameter(Name = "ClientPlayer")]
    public Player? ClientPlayer { get; set; }

    protected override void OnInitialized()
    {
        ViewModel.DetonateMines = EndGame;
        base.OnInitialized();
    }

    private void OnOpenCellClick(BoardSpace space)
    {
        if (ClientPlayer?.Id == GameState?.CurrentPlayer?.Id)
        {
            ViewModel.OpenCellCommand.Execute(space);
        }
    }

    private void EndGame()
    {
        var uri = Navigation.GetUriWithQueryParameters("/GameOver",
            new Dictionary<string, object?>()
            {
                { "Name", GameState!.CurrentPlayer!.Name },
                { "Score", GameState.CurrentPlayer.Score }
            });
        Navigation.NavigateTo(uri);
    }
    private static string GetCellColor(int num)
    {
        return num switch
        {
            1 => "Orchid",
            2 => "ForestGreen",
            3 => "FireBrick",
            4 => "MidnightBlue",
            5 => "Maroon",
            6 => "Aquamarine",
            7 => "DarkMagenta",
            8 => "DeepPink",
            _ => "Silver",
        };
    }

    private string GetPlayerFlag(string playerId)
    {
        if (ViewModel?.GameState is { } gameState && playerId is not null)
        {
            Player player = gameState.GetPlayer(playerId);
            return PlayerColorConverter.GetFlagImage(player.Color);
        }
        throw new InvalidOperationException();
    }
}
