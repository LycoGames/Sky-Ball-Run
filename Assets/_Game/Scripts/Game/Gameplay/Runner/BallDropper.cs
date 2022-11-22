using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class BallDropper : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BallMover ballMover)) ballMover.OnDropState();
        }
    }
}
