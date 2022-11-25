using _Game.Scripts.Base.UserInterface;
using _Game.Scripts.Game.UserInterfaces.Splash;
using UnityEngine;

namespace _Game.Scripts.Game.UserInterfaces.Loading
{
    public class PrepareGameCanvas : BaseCanvas, IStartable, IQuitable
    {
        [SerializeField] private LoadingText loadingText;

        public void OnStart()
        {
            EnableLoadingIcon();
            PlayLoadingIconAnimation();
        }

        public void OnQuit()
        {
            StopLoadingIconAnimation();
            DisableLoadingIcon();
        }
        private void EnableLoadingIcon()
        {
            loadingText.gameObject.SetActive(true);
        }

        private void DisableLoadingIcon()
        {
            loadingText.gameObject.SetActive(false);
        }
        private void PlayLoadingIconAnimation()
        {
            loadingText.PlayLoadingTextAnimation();
        }

        private void StopLoadingIconAnimation()
        {
            loadingText.StopLoadingTextAnimation();
        }
    }
}