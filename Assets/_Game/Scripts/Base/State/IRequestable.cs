namespace _Game.Scripts.Base.State
{
    public interface IRequestable
    {
        void SubscribeToCanvasRequestDelegates();
        void UnsubscribeToCanvasRequestDelegates();
    }
}