using System;
using _Game.Scripts.Game.Gameplay.Runner;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Waterfall
{
    public class WaterfallBasket : MonoBehaviour
    {
        public Action<int> GoldCollected;

        [SerializeField] private int pointMultiplier;
        [SerializeField] private TMP_Text multiplierText;

        private void OnEnable()
        {
            multiplierText.text = "X" + pointMultiplier;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Ball ball)) return;

            GoldCollected?.Invoke(pointMultiplier);
            // ball.ReturnToPool();
            other.GetComponent<MeshRenderer>().enabled = false;
            other.GetComponent<AudioSource>().Play();
        }
    }
}