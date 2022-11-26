using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.States.InGame;
using _Game.Scripts.Game.States.Splash;

namespace _Game.Scripts.Game.States
{
    public class AppState : StateMachine
    {
        private readonly SplashState splashState;
        private readonly GameState gameState;

        public AppState(ComponentContainer componentContainer)
        {
            splashState = new SplashState(componentContainer);
            gameState = new GameState(componentContainer);

            AddSubState(splashState);
            AddSubState(gameState);

            AddTransition(splashState, gameState, (int) StateTrigger.GoToGame);
        }
 
        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
        }
    }
}