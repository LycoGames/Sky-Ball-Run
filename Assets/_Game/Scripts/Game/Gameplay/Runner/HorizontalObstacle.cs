using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class HorizontalObstacle : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                meshRenderer.enabled = false;
            }
        }
    }
}
