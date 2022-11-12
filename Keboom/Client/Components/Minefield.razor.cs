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

    private static string GetCellColor(int num)
    {
        return num switch
        {
            1 => "DeepSkyBlue",
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
