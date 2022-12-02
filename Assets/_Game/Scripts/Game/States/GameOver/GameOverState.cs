using System.Collections;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.InGame;

namespace _Game.Scripts.Game.States.GameOver
{
    public class GameOverState : StateMachine, IRequestable
    {
        private readonly UIComponent uiComponent;
        private readonly GameOverComponent gameOverComponent;
        private readonly GameOverCanvas gameOverCanvas;

        public GameOverState(ComponentContainer componentContainer)
        {
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            gameOverComponent = componentContainer.GetComponent("GameOverComponent") as GameOverComponent;


            gameOverCanvas = uiComponent.GetCanvas(CanvasTrigger.GameOver) as GameOverCanvas;
        }

        protected override void OnEnter()
        {
            SubscribeToCanvasRequestDelegates();
            gameOverCanvas.OnStart();
            gameOverComponent.OnConstruct();

            uiComponent.EnableCanvas(CanvasTrigger.GameOver);
        }

        protected override void OnExit()
        {
            UnsubscribeToCanvasRequestDelegates();
            gameOverCanvas.OnQuit();
            gameOverComponent.OnConstruct();
        }

        public void SubscribeToCanvasRequestDelegates()
        {
            gameOverCanvas.OnReturnToMainRequest += RequestReturnToMain;
            gameOverComponent.GameOverComplete += ReturnToMain;
        }


        public void UnsubscribeToCanvasRequestDelegates()
        {
            gameOverCanvas.OnReturnToMainRequest -= RequestReturnToMain;
            gameOverComponent.GameOverComplete -= ReturnToMain;
        }

        private void RequestReturnToMain()
        {
            gameOverComponent.RemoveGame();
        }

        private void ReturnToMain()
        {
            SendTrigger((int)StateTrigger.ReturnToPreparingGame);
        }


    }
}