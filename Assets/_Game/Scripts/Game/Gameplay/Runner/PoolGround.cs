using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class PoolGround : MonoBehaviour
    {
        [SerializeField] private Checkpoint checkpoint;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                checkpoint.CollectBall();
            }
        }
    }
}
