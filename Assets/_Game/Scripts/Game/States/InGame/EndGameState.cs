using System.Collections;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.UserInterfaces.InGame;

namespace _Game.Scripts.Game.States.InGame
{
    public class EndGameState : StateMachine, IChangeable
    {
        private readonly UIComponent uiComponent;
        private readonly EndGameComponent endGameComponent;
        private readonly EndGameCanvas endGameCanvas;

        public EndGameState(ComponentContainer componentContainer)
        {
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            endGameComponent = componentContainer.GetComponent("EndGameComponent") as EndGameComponent;
            endGameCanvas = uiComponent.GetCanvas(CanvasTrigger.EndGame) as EndGameCanvas;
        }

        protected override void OnEnter()
        {
            SubscribeToComponentChangeDelegates();
            endGameCanvas.OnStart();
            endGameComponent.OnConstruct();

            uiComponent.EnableCanvas(CanvasTrigger.EndGame);
        }

        protected override void OnExit()
        {
            UnsubscribeToComponentChangeDelegates();
            endGameCanvas.OnQuit();
            endGameComponent.OnDestruct();
        }

        public void SubscribeToComponentChangeDelegates()
        {
            endGameComponent.CoinChange += endGameCanvas.ChangeCoin;
            endGameComponent.EndGameEnded += RequestGameOver;
        }

        public void UnsubscribeToComponentChangeDelegates()
        {
            endGameComponent.CoinChange -= endGameCanvas.ChangeCoin;
            endGameComponent.EndGameEnded -= RequestGameOver;
        }

        #region Changes

        #endregion

        private void RequestGameOver()
        {
            SendTrigger((int)StateTrigger.FinishEndGame);
        }
    }
}