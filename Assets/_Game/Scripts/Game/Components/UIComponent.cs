using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.UserInterface;
using _Game.Scripts.Game.Enums;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class UIComponent : MonoBehaviour, IComponent
    {
        [SerializeField] private BaseCanvas loadingGameCanvas;
        [SerializeField] private BaseCanvas prepareGameCanvas;
        [SerializeField] private BaseCanvas inGameCanvas;
        [SerializeField] private BaseCanvas endGameCanvas;

        private BaseCanvas activeCanvas;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            loadingGameCanvas.Initialize();
            prepareGameCanvas.Initialize();
            inGameCanvas.Initialize();
            endGameCanvas.Initialize();

            DeactivateCanvas(loadingGameCanvas);
            DeactivateCanvas(prepareGameCanvas);
            DeactivateCanvas(inGameCanvas);
            DeactivateCanvas(endGameCanvas);
        }

        public BaseCanvas GetCanvas(CanvasTrigger canvasTrigger)
        {
            return canvasTrigger switch
            {
                CanvasTrigger.LoadingGame => loadingGameCanvas,
                CanvasTrigger.PrepareGame => prepareGameCanvas,
                CanvasTrigger.InGame => inGameCanvas,
                CanvasTrigger.EndGame => endGameCanvas,
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
                CanvasTrigger.LoadingGame => loadingGameCanvas,
                CanvasTrigger.PrepareGame => prepareGameCanvas,
                CanvasTrigger.InGame => inGameCanvas,
                CanvasTrigger.EndGame => endGameCanvas,
                _ => activeCanvas
            };

            if (activeCanvas)
                activeCanvas.Activate();
        }
    }
}