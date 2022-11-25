using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.Loading;
using _Game.Scripts.Game.UserInterfaces.Splash;

namespace _Game.Scripts.Game.States.Splash
{
    public class LoadingGameState : StateMachine, IChangeable
    {
        private readonly UIComponent uiComponent;
        private readonly LoadingGameComponent loadingGameComponent;

        private readonly PrepareGameCanvas prepareGameCanvas;

        public LoadingGameState(ComponentContainer componentContainer)
        {
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            loadingGameComponent = componentContainer.GetComponent("LoadingGameComponent") as LoadingGameComponent;

            prepareGameCanvas = uiComponent.GetCanvas(CanvasTrigger.LoadingGame) as PrepareGameCanvas;
        }

        protected override void OnEnter()
        {
            SubscribeToComponentChangeDelegates();
            uiComponent.EnableCanvas(CanvasTrigger.LoadingGame);

            loadingGameComponent.OnConstruct();
            prepareGameCanvas.OnStart();
        }

        protected override void OnExit()
        {
            UnsubscribeToComponentChangeDelegates();
        }

        public void SubscribeToComponentChangeDelegates()
        {
            loadingGameComponent.OnLoadingSliderStart += StartLoadingSlider;
            loadingGameComponent.OnLoadingComplete += RequestPrepareGame;
        }

        public void UnsubscribeToComponentChangeDelegates()
        {
            loadingGameComponent.OnLoadingSliderStart -= StartLoadingSlider;
            loadingGameComponent.OnLoadingComplete -= RequestPrepareGame;
        }

        private void StartLoadingSlider(float time)
        {
            //loadingGameCanvas.StartSlider(time);
        }


        private void RequestPrepareGame()
        {
            SendTrigger((int) StateTrigger.GoToMain);
        }
    }
}