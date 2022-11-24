using _Game.Scripts.Base.UserInterface;
using _Game.Scripts.Game.UserInterfaces.Splash;
using UnityEngine;

namespace _Game.Scripts.Game.UserInterfaces.Loading
{
    public class LoadingGameCanvas : BaseCanvas, IStartable, IQuitable
    {
        [SerializeField] private LoadingText loadingText;

        public void OnStart()
        {
           
        }

        public void OnQuit()
        {
           
        }

       
    }
}