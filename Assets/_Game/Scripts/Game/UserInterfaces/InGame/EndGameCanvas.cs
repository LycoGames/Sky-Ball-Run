using _Game.Scripts.Base.UserInterface;

namespace _Game.Scripts.Game.UserInterfaces.InGame
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