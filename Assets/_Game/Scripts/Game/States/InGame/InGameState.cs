using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.InGame;
using Unity.VisualScripting.Dependencies.NCalc;

namespace _Game.Scripts.Game.States.InGame
{
    public class InGameState : StateMachine, IRequestable
    {
        private readonly UIComponent uiComponent;
        private readonly InGameComponent inGameComponent;
        private readonly InGameCanvas inGameCanvas;
        private readonly WealthCanvas wealthCanvas;

        public InGameState(ComponentContainer componentContainer)
        {
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            inGameComponent = componentContainer.GetComponent("InGameComponent") as InGameComponent;

            inGameCanvas = uiComponent.GetCanvas(CanvasTrigger.InGame) as InGameCanvas;
            wealthCanvas = uiComponent.GetCanvas(CanvasTrigger.Wealth) as WealthCanvas;
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
            UnsubscribeToComponentChangeDelegates();
            UnsubscribeToCanvasRequestDelegates();

            inGameCanvas.OnQuit();
            inGameComponent.OnDestruct();

            uiComponent.DisableCanvas(CanvasTrigger.InGame);
        }

        public void SubscribeToComponentChangeDelegates()
        {
            inGameComponent.DiamondChange += wealthCanvas.ChangeDiamond;
            inGameComponent.OnInGameComplete += RequestEndGame;
            inGameComponent.OnLoseGame += RequestGameOver;
        }


        public void UnsubscribeToComponentChangeDelegates()
        {
            inGameComponent.DiamondChange += wealthCanvas.ChangeDiamond;
            inGameComponent.OnInGameComplete -= RequestEndGame;
            inGameComponent.OnLoseGame -= RequestGameOver;
        }

        public void SubscribeToCanvasRequestDelegates()
        {
        }

        public void UnsubscribeToCanvasRequestDelegates()
        {
        }

        private void RequestGameOver()
        {
            SendTrigger((int)StateTrigger.GoToGameOver);
        }

        private void RequestEndGame()
        {
            SendTrigger((int)StateTrigger.FinishGame);
        }
    }
}