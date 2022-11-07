using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.InGame;

namespace _Game.Scripts.Game.States.InGame
{
    public class InGameState : StateMachine, IChangeable, IRequestable
    {
        private readonly UIComponent uiComponent;
        private readonly InGameComponent inGameComponent;
        private readonly InGameCanvas inGameCanvas;

        public InGameState(ComponentContainer componentContainer)
        {
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            inGameComponent = componentContainer.GetComponent("InGameComponent") as InGameComponent;

            inGameCanvas = uiComponent.GetCanvas(CanvasTrigger.InGame) as InGameCanvas;
        }

        protected override void OnEnter()
        {
            SubscribeToComponentChangeDelegates();
            SubscribeToCanvasRequestDelegates();

            inGameCanvas.OnStart();
            inGameComponent.OnConstruct();

            uiComponent.EnableCanvas(CanvasTrigger.InGame);
        }

        protected override void OnExit()
        {
            inGameComponent.OnDestruct();

            UnsubscribeToComponentChangeDelegates();
            UnsubscribeToCanvasRequestDelegates();
        }

        public void SubscribeToComponentChangeDelegates()
        {
            inGameComponent.OnInGameComplete += RequestEndGame;
        }


        public void UnsubscribeToComponentChangeDelegates()
        {
            inGameComponent.OnInGameComplete -= RequestEndGame;
        }

        public void SubscribeToCanvasRequestDelegates()
        {
        }

        public void UnsubscribeToCanvasRequestDelegates()
        {
        }

        private void ChangeLevel(string text, string txt)
        {
            inGameCanvas.ChangeCurrentLevel(text);
            inGameCanvas.ChangeNextLevel(txt);
        }

        private void RequestEndGame()
        {
            SendTrigger((int)StateTrigger.FinishGame);
        }
    }
}