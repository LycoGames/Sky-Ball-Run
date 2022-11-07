using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.Main;

namespace _Game.Scripts.Game.States.Main
{
    public class PrepareGameState : StateMachine, IRequestable, IChangeable
    {
        private readonly UIComponent uiComponent;
        private readonly PrepareGameComponent prepareGameComponent;

        private readonly PrepareGameCanvas prepareGameCanvas;

        public PrepareGameState(ComponentContainer componentContainer)
        {
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            prepareGameComponent = componentContainer.GetComponent("PrepareGameComponent") as PrepareGameComponent;

            prepareGameCanvas = uiComponent.GetCanvas(CanvasTrigger.PrepareGame) as PrepareGameCanvas;
        }

        protected override void OnEnter()
        {
            SubscribeToComponentChangeDelegates();
            SubscribeToCanvasRequestDelegates();

            prepareGameCanvas.OnStart();
            prepareGameComponent.OnConstruct();

            uiComponent.EnableCanvas(CanvasTrigger.PrepareGame);
        }

        protected override void OnExit()
        {
            prepareGameComponent.OnDestruct();

            UnsubscribeToComponentChangeDelegates();
            UnsubscribeToCanvasRequestDelegates();
        }

        #region Subscriptions

        public void SubscribeToComponentChangeDelegates()
        {
            prepareGameComponent.OnGameLaunch += RequestStartGame;
            prepareGameComponent.OnLevelChange += ChangeLevel;
        }

        public void UnsubscribeToComponentChangeDelegates()
        {
            prepareGameComponent.OnGameLaunch -= RequestStartGame;
            prepareGameComponent.OnLevelChange -= ChangeLevel;
        }

        public void SubscribeToCanvasRequestDelegates()
        {
            prepareGameCanvas.OnSettingsRequest += RequestSettings;
        }

        public void UnsubscribeToCanvasRequestDelegates()
        {
            prepareGameCanvas.OnSettingsRequest -= RequestSettings;
        }

        #endregion

        #region Changes

        private void ChangeLevel(string text, string txt)
        {
            prepareGameCanvas.SetCurrentLevelText(text);
            prepareGameCanvas.SetNextLevelText(txt);
        }

        #endregion

        #region Requests

        private void RequestSettings()
        {
            SendTrigger((int)StateTrigger.GoToSettings);
        }

        private void RequestStartGame()
        {
            SendTrigger((int)StateTrigger.StartGame);
        }

        #endregion
    }
}