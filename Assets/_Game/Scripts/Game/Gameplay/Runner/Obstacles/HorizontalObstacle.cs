using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace _Game.Scripts.Game.Gameplay.Runner.Obstacles
{
    public class HorizontalObstacle : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private float colorChangeTime = .3f;
        
        private void OnEnable()
        {
            Color color = material.color;
            color.a = 255;
            material.color = color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                material.DOFade(0, colorChangeTime);
            }
        }
    }
}
