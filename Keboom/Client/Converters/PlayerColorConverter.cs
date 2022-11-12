using Keboom.Shared;

namespace Keboom.Client.Converters;

public static class PlayerColorConverter
{
    public static string GetFlagImage(PlayerColor color)
        => color switch
        {
            PlayerColor.Red => "RedFlag.gif",
            PlayerColor.Blue => "BlueFlag.gif",
            PlayerColor.Green => "GreenFlag.gif",
            PlayerColor.Yellow => "YellowFlag.gif",
            _ => throw new ArgumentException($"Unknown player color '{color}'", nameof(color))
        };
}
