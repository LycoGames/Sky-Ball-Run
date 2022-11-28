using System;
using _Game.Scripts.Base.UserInterface;
using _Game.Scripts.Game.Gameplay.Runner;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.UserInterfaces.InGame
{
    public class EndGameCanvas : BaseCanvas, IStartable, IQuitable
    {
        public delegate void EndGameRequestDelegate();

        public event EndGameRequestDelegate OnReturnToMainRequest;

        [SerializeField] private TextMeshProUGUI endText;
        

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
            if(ballCount>0)ChangeTextYouWin();
            else ChangeTextYouLose();
        }
        

        #region Changes

        private void ChangeTextYouLose() => endText.text = "You Lose";
        private void ChangeTextYouWin() => endText.text = "You Win";
        #endregion

        public void RequestReturnToPreparingGame()
        {
            OnReturnToMainRequest?.Invoke();
        }

       
    }
}