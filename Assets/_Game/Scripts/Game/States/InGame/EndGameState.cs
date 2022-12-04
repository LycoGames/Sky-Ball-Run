using System.Collections;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.Gameplay.Runner;
using _Game.Scripts.Game.UserInterfaces.InGame;

namespace _Game.Scripts.Game.States.InGame
{
    public class EndGameState : StateMachine, IChangeable
    {
        private readonly UIComponent uiComponent;
        private readonly EndGameComponent endGameComponent;
        private readonly EndGameCanvas endGameCanvas;
        private readonly WealthCanvas wealthCanvas;

        public EndGameState(ComponentContainer componentContainer)
        {
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            endGameComponent = componentContainer.GetComponent("EndGameComponent") as EndGameComponent;
            endGameCanvas = uiComponent.GetCanvas(CanvasTrigger.EndGame) as EndGameCanvas;
            wealthCanvas = uiComponent.GetCanvas(CanvasTrigger.Wealth) as WealthCanvas;
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

            uiComponent.DisableCanvas(CanvasTrigger.EndGame);
            uiComponent.DisableCanvas(CanvasTrigger.Wealth);
        }

        public void SubscribeToComponentChangeDelegates()
        {
            endGameComponent.DiamondChange += wealthCanvas.ChangeDiamond;
            endGameComponent.OnEndGameEnded += RequestGameOver;
        }

        public void UnsubscribeToComponentChangeDelegates()
        {
            endGameComponent.DiamondChange -= wealthCanvas.ChangeDiamond;
            endGameComponent.OnEndGameEnded -= RequestGameOver;
        }

        #region Changes

        #endregion

        private void RequestGameOver()
        {
            AudioSourceController.Instance.PlaySoundType(SoundType.WinLevel);
            SendTrigger((int)StateTrigger.FinishEndGame);
        }
    }
}