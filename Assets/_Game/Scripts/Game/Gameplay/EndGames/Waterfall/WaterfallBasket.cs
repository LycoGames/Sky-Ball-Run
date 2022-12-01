using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Waterfall
{
    public class WaterfallBasket : MonoBehaviour
    {
        public Action<int> GoldCollected;

        [SerializeField] private int pointMultiplier;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Ball")) return;
            GoldCollected?.Invoke(pointMultiplier);
            other.gameObject.SetActive(false);
        }
    }
}