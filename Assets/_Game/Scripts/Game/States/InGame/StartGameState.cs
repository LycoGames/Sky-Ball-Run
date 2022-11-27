using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.InGame;
using UnityEngine;

namespace _Game.Scripts.Game.States.InGame
{
    public class StartGameState : StateMachine, IRequestable
    {
        private readonly UIComponent uiComponent;
        private readonly StartGameCanvas startGameCanvas;

        public StartGameState(ComponentContainer _componentContainer)
        {
            uiComponent=_componentContainer.GetComponent("UIComponent") as UIComponent;
            startGameCanvas = uiComponent.GetCanvas(CanvasTrigger.StartGame) as StartGameCanvas;
        }
        protected override void OnEnter()
        {
            SubscribeToCanvasRequestDelegates();

        }

        protected override void OnExit()
        {
            UnsubscribeToCanvasRequestDelegates();
        }

        public void SubscribeToCanvasRequestDelegates()
        {
            startGameCanvas.OnStartGameRequest += RequestStartGame;
        }

        public void UnsubscribeToCanvasRequestDelegates()
        {
            startGameCanvas.OnStartGameRequest -= RequestStartGame;
        }

        private void RequestStartGame()
        {
            Debug.Log("Game Start");
            SendTrigger((int)StateTrigger.StartGame);
        }
    }
}
