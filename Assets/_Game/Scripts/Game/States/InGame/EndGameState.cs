using Base.Component;
using Base.State;
using Game.Components;
using Game.Enums;
using Game.UserInterfaces.EndGame;
using UnityEngine;

namespace Game.States.InGame
{
    public class EndGameState : StateMachine, IChangeable, IRequestable
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
            SubscribeToCanvasRequestDelegates();

            endGameCanvas.OnStart();
            endGameComponent.OnConstruct();

            uiComponent.EnableCanvas(CanvasTrigger.EndGame);
        }

        protected override void OnExit()
        {
            UnsubscribeToComponentChangeDelegates();
            UnsubscribeToCanvasRequestDelegates();

            endGameCanvas.OnQuit();
            endGameComponent.OnDestruct();
        }

        public void SubscribeToComponentChangeDelegates()
        {
      
            endGameComponent.OnSuccess += Success;
            endGameComponent.OnFail += Fail;
        }


        public void UnsubscribeToComponentChangeDelegates()
        {

            endGameComponent.OnSuccess -= Success;
            endGameComponent.OnFail -= Fail;
        }

        public void SubscribeToCanvasRequestDelegates()
        {
            endGameCanvas.OnReturnToMainRequest += ReturnToMain;
        }


        public void UnsubscribeToCanvasRequestDelegates()
        {
            endGameCanvas.OnReturnToMainRequest -= ReturnToMain;
        }

        #region Changes

        private void ChangeFinalPhaseCannonballCount(string score, string targetScore)
        {
            endGameCanvas.ChangeCannonballCount(score, targetScore);
        }

        private void FinalPhase()
        {
            endGameCanvas.EnableFinalPhaseObjects();
        }

        private void EndGame()
        {
            endGameCanvas.EnableEndGameObjects();
        }

        private void Success()
        {
            endGameCanvas.EnableEndGameObjects();
            endGameCanvas.EnableSuccessObjects();
        }

        private void Fail()
        {
            endGameCanvas.EnableEndGameObjects();
            endGameCanvas.EnableFailObjects();
        }

        private void ChangeCompletedLevel(string completedLevel)
        {
            endGameCanvas.ChangeCompletedLevelText(completedLevel);
        }

        private void ChangeLevel(string currentLevel, string nextLevel)
        {
            endGameCanvas.ChangeCurrentLevel(currentLevel);
            endGameCanvas.ChangeNextLevel(nextLevel);
        }

        #endregion

        private void ReturnToMain()
        {
            SendTrigger((int)StateTrigger.ReturnToPreparingGame);
        }
    }
}