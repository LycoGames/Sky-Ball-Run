using System.Collections.Generic;
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
        [SerializeField] private BaseCanvas wealthCanvas;
        [SerializeField] private BaseCanvas endGameCanvas;
        [SerializeField] private BaseCanvas gameOverCanvas;

        private List<BaseCanvas> activeCanvases;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");

            introCanvas.Initialize();
            prepareGameCanvas.Initialize();
            startGameCanvas.Initialize();
            inGameCanvas.Initialize();
            wealthCanvas.Initialize();
            endGameCanvas.Initialize();
            gameOverCanvas.Initialize();
            InitializeActiveCanvasesList();

            DeactivateCanvas(introCanvas);
            DeactivateCanvas(prepareGameCanvas);
            DeactivateCanvas(startGameCanvas);
            DeactivateCanvas(inGameCanvas);
            DeactivateCanvas(wealthCanvas);
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
                CanvasTrigger.Wealth => wealthCanvas,
                CanvasTrigger.EndGame => endGameCanvas,
                CanvasTrigger.GameOver => gameOverCanvas,
                _ => null
            };
        }

        public void EnableCanvas(CanvasTrigger canvasTrigger)
        {
            ActivateCanvas(canvasTrigger);
        }

        public void DisableCanvas(CanvasTrigger canvasTrigger)
        {
            DeactivateCanvas(GetCanvas(canvasTrigger));
        }

        private void DeactivateCanvas(BaseCanvas canvas)
        {
            if (!activeCanvases.Contains(canvas)) return;
            canvas.Deactivate();
            activeCanvases.Remove(canvas);
        }

        private void ActivateCanvas(CanvasTrigger canvasTrigger)
        {
            var canvas = GetCanvas(canvasTrigger);

            if (activeCanvases.Contains(canvas)) return;
            canvas.Activate();
            activeCanvases.Add(canvas);
        }

        private void InitializeActiveCanvasesList()
        {
            activeCanvases = new List<BaseCanvas>()
            {
                introCanvas, prepareGameCanvas, startGameCanvas, inGameCanvas, wealthCanvas, endGameCanvas,
                gameOverCanvas
            };
        }
    }
}