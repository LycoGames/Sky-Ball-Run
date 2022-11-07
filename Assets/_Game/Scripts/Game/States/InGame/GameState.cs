using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Enums;

namespace _Game.Scripts.Game.States.InGame
{
    public class GameState : StateMachine
    {
        private readonly InGameState inGameState;
        private readonly EndGameState endGameState;

        public GameState(ComponentContainer componentContainer)
        {
            inGameState = new InGameState(componentContainer);
            endGameState = new EndGameState(componentContainer);

            AddSubState(inGameState);
            AddSubState(endGameState);

            AddTransition(inGameState, endGameState, (int) StateTrigger.FinishGame);
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