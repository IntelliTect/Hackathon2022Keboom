namespace Keboom.Shared
{
    public interface IGameHubEventHandler
    {
        void LostConnection();
        void Connected();
    }
}
