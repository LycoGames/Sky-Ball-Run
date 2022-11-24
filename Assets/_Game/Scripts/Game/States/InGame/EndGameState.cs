using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.EndGame;

namespace _Game.Scripts.Game.States.InGame
{
    public class EndGameState : StateMachine, IChangeable, IRequestable
    {
        private readonly UIComponent uiComponent;
        private readonly EndGameComponent endGameComponent;

        private readonly EndGameCanvas endGameCanvas;

        public EndGameState(ComponentContainer componentContainer)
        {
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            endGameComponent = componentContainer.GetComponent("EndGameComponent") as EndGameComponent;

            endGameCanvas = uiComponent.GetCanvas(CanvasTrigger.EndGame) as EndGameCanvas;
        }

        protected override void OnEnter()
        {
            SubscribeToComponentChangeDelegates();
            SubscribeToCanvasRequestDelegates();

            endGameCanvas.OnStart();
            endGameComponent.OnConstruct();

            uiComponent.EnableCanvas(CanvasTrigger.EndGame);
        }

        protected override void OnExit()
        {
            UnsubscribeToComponentChangeDelegates();
            UnsubscribeToCanvasRequestDelegates();

            endGameCanvas.OnQuit();
            endGameComponent.OnDestruct();
        }

        public void SubscribeToComponentChangeDelegates()
        {
      
 
        }


        public void UnsubscribeToComponentChangeDelegates()
        {

   
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
            SendTrigger((int)StateTrigger.ReturnToPreparingGame);
        }
    }
}