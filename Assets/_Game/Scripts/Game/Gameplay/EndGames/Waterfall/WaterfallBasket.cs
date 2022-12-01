using System;
using _Game.Scripts.Game.Gameplay.Runner;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Waterfall
{
    public class WaterfallBasket : MonoBehaviour
    {
        public Action<int> GoldCollected;

        [SerializeField] private int pointMultiplier;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Ball ball)) return;

            GoldCollected?.Invoke(pointMultiplier);
            ball.ReturnToPool();
        }
    }
}