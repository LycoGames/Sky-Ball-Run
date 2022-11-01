using Base.Component;
using Base.State;
using Game.Enums;
using UnityEngine;

namespace Game.States.InGame
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