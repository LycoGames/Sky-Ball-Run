using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.Loading;
using _Game.Scripts.Game.UserInterfaces.Splash;

namespace _Game.Scripts.Game.States.Splash
{
    public class PrepareGameState : StateMachine, IChangeable
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
            SubscribeToComponentChangeDelegates();
            
            prepareGameComponent.OnConstruct();
            prepareGameCanvas.OnStart();
            
            uiComponent.EnableCanvas(CanvasTrigger.PrepareGame);
        }

        protected override void OnExit()
        {
            prepareGameComponent.OnDestruct();
            prepareGameCanvas.OnQuit();

            UnsubscribeToComponentChangeDelegates();
        }

        public void SubscribeToComponentChangeDelegates()
        {
        }

        public void UnsubscribeToComponentChangeDelegates()
        {
        }

        #region Changes

        private void RequestStartGame()
        {
            SendTrigger((int) StateTrigger.StartGame);
        }
        #endregion
    }
}