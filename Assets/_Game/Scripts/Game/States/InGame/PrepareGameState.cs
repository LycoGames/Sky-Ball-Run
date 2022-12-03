using System.Collections;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.InGame;
using _Game.Scripts.Game.UserInterfaces.Loading;
using _Game.Scripts.Game.UserInterfaces.Splash;

namespace _Game.Scripts.Game.States.Splash
{
    public class PrepareGameState : StateMachine, IRequestable
    {
        private readonly UIComponent uiComponent;
        private readonly PrepareGameComponent prepareGameComponent;
        private readonly PrepareGameCanvas prepareGameCanvas;
        private readonly ComponentContainer componentContainer;
        
        private readonly WealthCanvas wealthCanvas;

        public PrepareGameState(ComponentContainer _componentContainer)
        {
            componentContainer = _componentContainer;
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            prepareGameComponent = componentContainer.GetComponent("PrepareGameComponent") as PrepareGameComponent;
            prepareGameCanvas = uiComponent.GetCanvas(CanvasTrigger.PrepareGame) as PrepareGameCanvas;
            wealthCanvas = uiComponent.GetCanvas(CanvasTrigger.Wealth) as WealthCanvas;
        }

        protected override void OnEnter()
        {
            SubscribeToCanvasRequestDelegates();
            prepareGameComponent.OnConstruct();
            prepareGameCanvas.OnStart();

            uiComponent.EnableCanvas(CanvasTrigger.PrepareGame);
        }

        protected override void OnExit()
        {
            UnsubscribeToCanvasRequestDelegates();
            prepareGameCanvas.OnQuit();

            uiComponent.DisableCanvas(CanvasTrigger.PrepareGame);
        }


        #region Changes

        private void RequestLoadGame()
        {
            SendTrigger((int)StateTrigger.GoToStartGame);
            uiComponent.EnableCanvas(CanvasTrigger.StartGame);
        }

        #endregion

        public void SubscribeToCanvasRequestDelegates()
        {
            prepareGameComponent.OnGameLaunch += RequestLoadGame;
            prepareGameComponent.SetLevelOnCanvas += wealthCanvas.SetupLevel;
            prepareGameComponent.DiamondChange += wealthCanvas.ChangeDiamond;
        }

        public void UnsubscribeToCanvasRequestDelegates()
        {
            prepareGameComponent.OnGameLaunch -= RequestLoadGame;
            prepareGameComponent.SetLevelOnCanvas -= wealthCanvas.SetupLevel;
            prepareGameComponent.DiamondChange -= wealthCanvas.ChangeDiamond;
        }
    }
}