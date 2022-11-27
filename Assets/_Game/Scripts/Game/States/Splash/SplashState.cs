using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;

namespace _Game.Scripts.Game.States.Splash
{
    public class SplashState : StateMachine
    {
        private readonly IntroState introState;

        public SplashState(ComponentContainer componentContainer)
        {
            introState = new IntroState(componentContainer);
            AddSubState(introState);
        }

        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
        }
    }
}