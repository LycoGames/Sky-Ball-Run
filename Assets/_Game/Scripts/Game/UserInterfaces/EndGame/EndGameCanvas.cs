using _Game.Scripts.Base.UserInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Game.UserInterfaces.EndGame
{
    public class EndGameCanvas : BaseCanvas, IStartable, IQuitable
    {
        public delegate void EndGameRequestDelegate();

        public event EndGameRequestDelegate OnReturnToMainRequest;


        

        public void OnStart()
        {
           
        }


        public void OnQuit()
        {
        }

        #region Changes

        #endregion

        public void RequestReturnToPreparingGame()
        {
            OnReturnToMainRequest?.Invoke();
        }

       
    }
}