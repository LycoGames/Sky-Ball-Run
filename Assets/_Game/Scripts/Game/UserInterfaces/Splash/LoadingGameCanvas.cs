using Base.UserInterface;
using UnityEngine;

namespace Game.UserInterfaces.Splash
{
    public class LoadingGameCanvas : BaseCanvas, IStartable, IQuitable
    {
        [SerializeField] private LoadingSlider loadingSlider;

        public void OnStart()
        {
            EnableLoadingSlider();
        }

        public void OnQuit()
        {
            DisableLoadingSlider();
        }

        private void EnableLoadingSlider()
        {
            loadingSlider.gameObject.SetActive(true);
        }

        private void DisableLoadingSlider()
        {
            loadingSlider.gameObject.SetActive(false);
        }

        public void StartSlider(float time)
        {
            loadingSlider.PlaySlider(time);
        }
    }
}