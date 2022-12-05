using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private Transform destroyedTransform;
        private bool isFirstTouch=true;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball") && isFirstTouch)
            {
                isFirstTouch = false;
                GameManager.Instance.OnRevive += DestroyObstacle;
            }
        }

        private void DestroyObstacle()
        {
            GameManager.Instance.OnRevive -= DestroyObstacle;
            Destroy(destroyedTransform.gameObject);
        }
    }
}
