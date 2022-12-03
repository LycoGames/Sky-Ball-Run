using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.InGame;
using UnityEngine;

namespace _Game.Scripts.Game.States.InGame
{
    public class StartGameState : StateMachine, IRequestable, IChangeable
    {
        private readonly UIComponent uiComponent;
        private readonly StartGameComponent startGameComponent;

        private readonly StartGameCanvas startGameCanvas;
        private readonly WealthCanvas wealthCanvas;

        public StartGameState(ComponentContainer _componentContainer)
        {
            uiComponent = _componentContainer.GetComponent("UIComponent") as UIComponent;
            startGameComponent = _componentContainer.GetComponent("StartGameComponent") as StartGameComponent;

            startGameCanvas = uiComponent.GetCanvas(CanvasTrigger.StartGame) as StartGameCanvas;
            wealthCanvas = uiComponent.GetCanvas(CanvasTrigger.Wealth) as WealthCanvas;
        }

        protected override void OnEnter()
        {
            SubscribeToComponentChangeDelegates();
            SubscribeToCanvasRequestDelegates();

            startGameComponent.OnConstruct();
            startGameCanvas.OnStart();

            uiComponent.EnableCanvas(CanvasTrigger.StartGame);
            uiComponent.EnableCanvas(CanvasTrigger.Wealth);
        }

        protected override void OnExit()
        {
            UnsubscribeToComponentChangeDelegates();
            UnsubscribeToCanvasRequestDelegates();

            startGameCanvas.OnQuit();

            uiComponent.DisableCanvas(CanvasTrigger.StartGame);
        }

        public void SubscribeToComponentChangeDelegates()
        {
            startGameComponent.CoinChange += wealthCanvas.SetupCoin;
            startGameComponent.DiamondChange += wealthCanvas.SetupDiamond;
        }

        public void UnsubscribeToComponentChangeDelegates()
        {
            startGameComponent.CoinChange -= wealthCanvas.SetupCoin;
            startGameComponent.DiamondChange -= wealthCanvas.SetupDiamond;
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