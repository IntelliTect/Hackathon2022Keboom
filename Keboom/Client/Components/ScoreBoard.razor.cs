using Keboom.Shared;
using Microsoft.AspNetCore.Components;

namespace Keboom.Client.Components;

public partial class ScoreBoard
{
    [CascadingParameter(Name = "GameState")]
    public GameState? GameState
    {
        get => ViewModel.GameState;
        set => ViewModel.GameState = value;
    }
}
