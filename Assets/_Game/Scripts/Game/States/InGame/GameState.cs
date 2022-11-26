using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.States.Splash;

namespace _Game.Scripts.Game.States.InGame
{
    public class GameState : StateMachine
    {
        private readonly PrepareGameState prepareGameState;
        private readonly StartGameState startGameState;
        private readonly InGameState inGameState;
        private readonly EndGameState endGameState;

        public GameState(ComponentContainer componentContainer)
        {
            prepareGameState = new PrepareGameState(componentContainer);
            startGameState = new StartGameState(componentContainer);
            inGameState = new InGameState(componentContainer);
            endGameState = new EndGameState(componentContainer);

            AddSubState(prepareGameState);
            AddSubState(startGameState);
            AddSubState(inGameState);
            AddSubState(endGameState);

            AddTransition(prepareGameState, startGameState, (int) StateTrigger.GoToStartGame);
            AddTransition(startGameState, inGameState, (int) StateTrigger.StartGame);
            AddTransition(inGameState, endGameState, (int) StateTrigger.FinishGame);
            AddTransition(endGameState, prepareGameState, (int) StateTrigger.ReturnToPreparingGame);
        }

        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
            SetCurrentSubStateToDefaultSubState();
        }
    }
}