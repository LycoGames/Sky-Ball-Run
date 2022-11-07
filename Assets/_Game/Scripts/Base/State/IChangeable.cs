namespace _Game.Scripts.Base.State
{
    public interface IChangeable
    {
        void SubscribeToComponentChangeDelegates();
        void UnsubscribeToComponentChangeDelegates();
    }
}