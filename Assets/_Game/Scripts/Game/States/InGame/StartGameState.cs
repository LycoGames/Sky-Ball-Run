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
   

        public StartGameState(ComponentContainer _componentContainer)
        {
            uiComponent = _componentContainer.GetComponent("UIComponent") as UIComponent;
            startGameComponent = _componentContainer.GetComponent("StartGameComponent") as StartGameComponent;

            startGameCanvas = uiComponent.GetCanvas(CanvasTrigger.StartGame) as StartGameCanvas;
          
        }

        protected override void OnEnter()
        {
          
            startGameComponent.OnConstruct();
            startGameCanvas.OnStart();
            SubscribeToCanvasRequestDelegates();
            uiComponent.EnableCanvas(CanvasTrigger.StartGame);
            uiComponent.EnableCanvas(CanvasTrigger.Wealth);
        }

        protected override void OnExit()
        {
            startGameCanvas.OnQuit();
            UnsubscribeToCanvasRequestDelegates();
            uiComponent.DisableCanvas(CanvasTrigger.StartGame);
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