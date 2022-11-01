using Base.State;
using Base.Component;
using Game.Components;
using Game.Enums;
using Game.UserInterfaces.InGame;
using UnityEngine;

namespace Game.States.InGame
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