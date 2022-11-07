using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;

namespace _Game.Scripts.Game.States.Splash
{
    public class SplashState : StateMachine
    {
        private readonly LoadingGameState loadingGameState;

        public SplashState(ComponentContainer componentContainer)
        {
            loadingGameState = new LoadingGameState(componentContainer);
            AddSubState(loadingGameState);
        }

        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
        }
    }
}