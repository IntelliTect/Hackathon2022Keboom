namespace Keboom.Shared
{
    public interface IGameHubEventHandler
    {
        event EventHandler ConnectionLost;
        event EventHandler Connected;
    }
}
