using System;
using _Game.Scripts.Game.Gameplay.EndGames.Waterfall;
using _Game.Scripts.Game.Gameplay.Runner;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.FlooredEndGame
{
    public class Floor : MonoBehaviour
    {
        [SerializeField] private int pointMultiplier;
        public Action<int> OnBallHit;
        public Action OnFirstHit;
        private bool isFirstHit=true;
        public DiamondRewardVisualizer DiamondRewardVisualizer { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                if (isFirstHit)
                {
                    isFirstHit = false;
                    OnFirstHit?.Invoke();
                }
                ball.RemoveBallWithoutRemoveFromList();
                DiamondRewardVisualizer.DiamondRewardSequence(other.transform.position, pointMultiplier);
                OnBallHit?.Invoke(pointMultiplier);
            }
        }
    }
}
