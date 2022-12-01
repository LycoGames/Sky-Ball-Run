using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.States.GameOver;
using _Game.Scripts.Game.States.Splash;

namespace _Game.Scripts.Game.States.InGame
{
    public class GameState : StateMachine
    {
        private readonly PrepareGameState prepareGameState;
        private readonly StartGameState startGameState;
        private readonly InGameState inGameState;
        private readonly EndGameState endGameState;
        private readonly GameOverState gameOverState;

        public GameState(ComponentContainer componentContainer)
        {
            prepareGameState = new PrepareGameState(componentContainer);
            startGameState = new StartGameState(componentContainer);
            inGameState = new InGameState(componentContainer);
            endGameState = new EndGameState(componentContainer);
            gameOverState = new GameOverState(componentContainer);

            AddSubState(prepareGameState);
            AddSubState(startGameState);
            AddSubState(inGameState);
            AddSubState(endGameState);
            AddSubState(gameOverState);

            AddTransition(prepareGameState, startGameState, (int)StateTrigger.GoToStartGame);
            AddTransition(startGameState, inGameState, (int)StateTrigger.StartGame);
            AddTransition(inGameState, endGameState, (int)StateTrigger.FinishGame);
            AddTransition(inGameState, gameOverState, (int)StateTrigger.GoToGameOver);
            AddTransition(endGameState, gameOverState, (int)StateTrigger.FinishEndGame);
            AddTransition(gameOverState, prepareGameState, (int)StateTrigger.ReturnToPreparingGame);
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