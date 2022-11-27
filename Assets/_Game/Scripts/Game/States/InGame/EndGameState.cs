using System.Collections;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.InGame;

namespace _Game.Scripts.Game.States.InGame
{
    public class EndGameState : StateMachine, IRequestable
    {
        private readonly UIComponent uiComponent;
        private readonly EndGameComponent endGameComponent;
        private readonly InGameComponent inGameComponent;
        private readonly EndGameCanvas endGameCanvas;

        public EndGameState(ComponentContainer componentContainer)
        {
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            endGameComponent = componentContainer.GetComponent("EndGameComponent") as EndGameComponent;
            inGameComponent = componentContainer.GetComponent("InGameComponent") as InGameComponent;
            endGameCanvas = uiComponent.GetCanvas(CanvasTrigger.EndGame) as EndGameCanvas;
        }

        protected override void OnEnter()
        {
            SubscribeToCanvasRequestDelegates();

            endGameCanvas.OnStart();
            endGameComponent.OnConstruct();

            uiComponent.EnableCanvas(CanvasTrigger.EndGame);
        }

        protected override void OnExit()
        {
            UnsubscribeToCanvasRequestDelegates();

            endGameCanvas.OnQuit();
            endGameComponent.OnDestruct();
        }
        

        public void SubscribeToCanvasRequestDelegates()
        {
            endGameCanvas.OnReturnToMainRequest += ReturnToMain;
        }


        public void UnsubscribeToCanvasRequestDelegates()
        {
            endGameCanvas.OnReturnToMainRequest -= ReturnToMain;
        }

        #region Changes

        
        #endregion

        private void ReturnToMain()
        {
            inGameComponent.GameManager.ResetGame();
            SendTrigger((int)StateTrigger.ReturnToPreparingGame);
        }
    }
}