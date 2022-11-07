using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.States.InGame;
using _Game.Scripts.Game.States.Main;
using _Game.Scripts.Game.States.Splash;

namespace _Game.Scripts.Game.States
{
    public class AppState : StateMachine
    {
        private readonly SplashState splashState;
        private readonly MainState mainState;
        private readonly GameState gameState;

        public AppState(ComponentContainer componentContainer)
        {
            splashState = new SplashState(componentContainer);
            mainState = new MainState(componentContainer);
            gameState = new GameState(componentContainer);

            AddSubState(splashState);
            AddSubState(mainState);
            AddSubState(gameState);

            AddTransition(splashState, mainState, (int) StateTrigger.GoToMain);
            AddTransition(mainState, gameState, (int) StateTrigger.StartGame);
            AddTransition(gameState, mainState, (int) StateTrigger.ReturnToPreparingGame);
        }

        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
        }
    }
}