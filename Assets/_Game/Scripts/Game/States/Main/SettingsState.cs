using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;

namespace _Game.Scripts.Game.States.Main
{
    public class SettingsState : StateMachine, IChangeable, IRequestable
    {

        public SettingsState(ComponentContainer componentContainer)
        {
            
        }
        protected override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeToComponentChangeDelegates()
        {
            throw new System.NotImplementedException();
        }

        public void UnsubscribeToComponentChangeDelegates()
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeToCanvasRequestDelegates()
        {
            throw new System.NotImplementedException();
        }

        public void UnsubscribeToCanvasRequestDelegates()
        {
            throw new System.NotImplementedException();
        }
    }
}