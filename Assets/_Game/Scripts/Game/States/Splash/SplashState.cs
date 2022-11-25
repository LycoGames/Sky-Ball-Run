using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;

namespace _Game.Scripts.Game.States.Splash
{
    public class SplashState : StateMachine
    {
        private readonly PrepareGameState prepareGameState;

        public SplashState(ComponentContainer componentContainer)
        {
            prepareGameState = new PrepareGameState(componentContainer);
            AddSubState(prepareGameState);
        }

        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
        }
    }
}