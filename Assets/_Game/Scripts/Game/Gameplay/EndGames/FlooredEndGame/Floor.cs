using System;
using _Game.Scripts.Game.Gameplay.EndGames.Waterfall;
using _Game.Scripts.Game.Gameplay.Runner;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.FlooredEndGame
{
    public class Floor : MonoBehaviour
    {
        [SerializeField] private int pointMultiplier;
        [SerializeField] private bool isLastFloor;
        public Action<int> OnBallHit;
        public Action OnFirstHit;
        private bool isFirstHit=true;
        public DiamondRewardVisualizer DiamondRewardVisualizer { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                if (isFirstHit&&!isLastFloor)
                {
                    isFirstHit = false;
                    OnFirstHit?.Invoke();
                }
                ball.RemoveBallWithoutRemoveFromList();
                DiamondRewardVisualizer.DiamondRewardSequence(other.transform.position, 1);
                OnBallHit?.Invoke(pointMultiplier);
            }
        }
    }
}
