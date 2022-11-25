using _Game.Scripts.Base.UserInterface;
using UnityEngine;

namespace _Game.Scripts.Game.UserInterfaces.Splash
{
    public class IntroCanvas : BaseCanvas, IStartable, IQuitable
    {
        [SerializeField] private Logo logo;

        public void OnStart()
        {
            EnableLogo();
        }

        public void OnQuit()
        {
            DisableLogo();
        }

       
        public void PlayLogoFadeOutAnimation(float animationTime)
        {
            logo.PlayFadeOutAnimation(animationTime);
        }

        private void EnableLogo()
        {
            logo.gameObject.SetActive(true);
        }

       
        private void DisableLogo()
        {
            logo.gameObject.SetActive(false);
        }
        
    }
}
