using _Game.Scripts.Base.UserInterface;
using UnityEngine;

namespace _Game.Scripts.Game.UserInterfaces.InGame
{
    public class StartGameCanvas : BaseCanvas, IStartable, IQuitable
    {
        public delegate void StartGameRequestDelegate();

        public event StartGameRequestDelegate OnStartGameRequest;

        public void OnStart()
        {
        }

        public void OnQuit()
        {
        }

        public void RequestStartGame()
        {
            OnStartGameRequest?.Invoke();
        }
       
    }
}
