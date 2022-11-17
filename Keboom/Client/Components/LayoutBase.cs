using Microsoft.AspNetCore.Components;

namespace Keboom.Client.Components;

public class LayoutBase : LayoutComponentBase
{
    [Parameter]
    public string? PageTitle { get; set; } = "KeBOOM";
}
