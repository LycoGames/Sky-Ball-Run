using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class PlayerStoper : MonoBehaviour
    {
        [SerializeField] private Checkpoint checkpoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.GetPlayerController().StopMove();
                checkpoint.StartCollectingBalls();
                gameObject.SetActive(false);
            }
        }
    }
}
