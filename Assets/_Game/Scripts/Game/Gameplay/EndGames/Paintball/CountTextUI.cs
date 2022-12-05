using System;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Paintball
{
    public class CountTextUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text countText;
        private Canvas canvas;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
            Deactivate();
        }

        public void Activate()
        {
            if (!canvas.enabled)
                canvas.enabled = true;
        }

        public void ChangeCount(int value)
        {
            countText.text = value.ToString();
        }

        private void Deactivate()
        {
            canvas.enabled = false;
        }
    }
}