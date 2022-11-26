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
        private readonly StartGameComponent startGameComponent;
        private readonly StartGameCanvas startGameCanvas;
        private readonly ComponentContainer componentContainer;

        public StartGameState(ComponentContainer _componentContainer)
        {
            componentContainer = _componentContainer;
            uiComponent=_componentContainer.GetComponent("UIComponent") as UIComponent;
            startGameComponent=_componentContainer.GetComponent("StartGameComponent") as StartGameComponent;
            startGameCanvas=_componentContainer.GetComponent("StartGameCanvas") as StartGameCanvas;
        }
        protected override void OnEnter()
        {
            SubscribeToCanvasRequestDelegates();

        }

        protected override void OnExit()
        {
            UnsubscribeToCanvasRequestDelegates();
            InGameComponent inGameComponent = componentContainer.GetComponent("InGameComponent") as InGameComponent;
            
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
            SendTrigger((int)StateTrigger.GoToGame);
        }
    }
}
