using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.Loading;
using _Game.Scripts.Game.UserInterfaces.Splash;

namespace _Game.Scripts.Game.States.Splash
{
    public class PrepareGameState : StateMachine ,IRequestable
    {
        private readonly UIComponent uiComponent;
        private readonly PrepareGameComponent prepareGameComponent;
        private readonly PrepareGameCanvas prepareGameCanvas;

        public PrepareGameState(ComponentContainer componentContainer)
        {
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            prepareGameComponent = componentContainer.GetComponent("LoadingGameComponent") as PrepareGameComponent;
            prepareGameCanvas = uiComponent.GetCanvas(CanvasTrigger.PrepareGame) as PrepareGameCanvas;
        }

        protected override void OnEnter()
        {
            prepareGameComponent.OnConstruct();
            prepareGameCanvas.OnStart();
            
            uiComponent.EnableCanvas(CanvasTrigger.PrepareGame);
        }

        protected override void OnExit()
        {
            prepareGameCanvas.OnQuit();
        }
        

        #region Changes

        private void RequestStartGame()
        {
            SendTrigger((int) StateTrigger.StartGame);
        }
        #endregion

        public void SubscribeToCanvasRequestDelegates()
        {
            prepareGameComponent.OnGameLaunch += RequestLoadGame;
        }

        public void UnsubscribeToCanvasRequestDelegates()
        {
            prepareGameComponent.OnGameLaunch -= RequestLoadGame;
        }
        private void RequestLoadGame()
        {
            SendTrigger((int)StateTrigger.GoToStartGame);
        }
    }
}