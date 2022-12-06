using System;
using _Game.Scripts.Base.UserInterface;
using _Game.Scripts.Game.Gameplay.Runner;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.UserInterfaces.InGame
{
    public class GameOverCanvas : BaseCanvas, IStartable, IQuitable
    {
        public delegate void EndGameRequestDelegate();

        public event EndGameRequestDelegate OnReturnToMainRequest;
        public event EndGameRequestDelegate OnReviveRequest;

        [SerializeField] private TextMeshProUGUI endText;
        [SerializeField] private TextMeshProUGUI nextLevelButtonText;
        [SerializeField] private GameObject reviveButton;


        public void OnStart()
        {
            SetEndText();
        }


        public void OnQuit()
        {
        }

        private void SetEndText()
        {
            int ballCount = GameManager.Instance.GetBallCount();
            if (ballCount > 0) NextLevelState();
            else LoseState();
        }


        #region Changes

        private void LoseState()
        { 
            endText.text = "You Lose";
            nextLevelButtonText.text = "Retry";
            reviveButton.SetActive(true);
        }
        private void NextLevelState()
        {
            endText.text = "You Win";
            nextLevelButtonText.text = "Next>";
            reviveButton.SetActive(false);
        }

        #endregion

        public void RequestRevive()
        {
            OnReviveRequest?.Invoke();
        }

        public void RequestReturnToPreparingGame()
        {
            OnReturnToMainRequest?.Invoke();
        }
    }
}