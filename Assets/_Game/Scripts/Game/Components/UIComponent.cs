using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.UserInterface;
using _Game.Scripts.Game.Enums;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class UIComponent : MonoBehaviour, IComponent
    {
        [SerializeField] private BaseCanvas introCanvas;
        [SerializeField] private BaseCanvas prepareGameCanvas;
        [SerializeField] private BaseCanvas startGameCanvas;
        [SerializeField] private BaseCanvas inGameCanvas;
        [SerializeField] private BaseCanvas endGameCanvas;
        [SerializeField] private BaseCanvas gameOverCanvas;

        private BaseCanvas activeCanvas;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            introCanvas.Initialize();
            prepareGameCanvas.Initialize();
            startGameCanvas.Initialize();
            inGameCanvas.Initialize();
            endGameCanvas.Initialize();
            gameOverCanvas.Initialize();

            DeactivateCanvas(introCanvas);
            DeactivateCanvas(prepareGameCanvas);
            DeactivateCanvas(startGameCanvas);
            DeactivateCanvas(inGameCanvas);
            DeactivateCanvas(endGameCanvas);
            DeactivateCanvas(gameOverCanvas);
        }

        public BaseCanvas GetCanvas(CanvasTrigger canvasTrigger)
        {
            return canvasTrigger switch
            {
                CanvasTrigger.Intro => introCanvas,
                CanvasTrigger.PrepareGame => prepareGameCanvas,
                CanvasTrigger.StartGame => startGameCanvas,
                CanvasTrigger.InGame => inGameCanvas,
                CanvasTrigger.EndGame => endGameCanvas,
                CanvasTrigger.GameOver => gameOverCanvas,
                _ => null
            };
        }

        public void EnableCanvas(CanvasTrigger canvasTrigger)
        {
            DeactivateCanvas(activeCanvas);
            ActivateCanvas(canvasTrigger);
        }

        private void DeactivateCanvas(BaseCanvas canvas)
        {
            if (canvas)
                canvas.Deactivate();
        }

        private void ActivateCanvas(CanvasTrigger canvasTrigger)
        {
            activeCanvas = canvasTrigger switch
            {
                CanvasTrigger.Intro => introCanvas,
                CanvasTrigger.PrepareGame => prepareGameCanvas,
                CanvasTrigger.StartGame => startGameCanvas,
                CanvasTrigger.InGame => inGameCanvas,
                CanvasTrigger.EndGame => endGameCanvas,
                CanvasTrigger.GameOver => gameOverCanvas,
                _ => activeCanvas
            };

            if (activeCanvas)
                activeCanvas.Activate();
        }
    }
}