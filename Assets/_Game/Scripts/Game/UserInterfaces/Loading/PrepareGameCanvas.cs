using _Game.Scripts.Base.UserInterface;
using _Game.Scripts.Game.UserInterfaces.Splash;
using UnityEngine;

namespace _Game.Scripts.Game.UserInterfaces.Loading
{
    public class PrepareGameCanvas : BaseCanvas, IStartable, IQuitable
    {
        [SerializeField] private LoadingImage loadingImage;

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
            loadingImage.gameObject.SetActive(true);
        }

        private void DisableLoadingIcon()
        {
            loadingImage.gameObject.SetActive(false);
        }
        private void PlayLoadingIconAnimation()
        {
            loadingImage.PlayLoadingTextAnimation();
        }

        private void StopLoadingIconAnimation()
        {
            loadingImage.StopLoadingTextAnimation();
        }
        
    }
}