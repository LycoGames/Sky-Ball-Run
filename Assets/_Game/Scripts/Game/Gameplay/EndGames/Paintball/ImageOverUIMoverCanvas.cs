using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Game.Gameplay.EndGames.Paintball
{
    public class ImageOverUIMoverCanvas : MonoBehaviour
    {
        [SerializeField] private Image image;

        private Canvas canvas;
        private Camera cam;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
            Deactivate();
        }

        public void Setup(Camera _cam)
        {
            cam = _cam;
        }

        public void Activate()
        {
            if (canvas.enabled) return;
            canvas.enabled = true;
        }

        public void Deactivate()
        {
            if (canvas.enabled)
                canvas.enabled = false;
        }

        public void ChangeImagePosition(Vector3 worldPosition)
        {
            image.rectTransform.position = cam.WorldToScreenPoint(worldPosition);
        }
    }
}