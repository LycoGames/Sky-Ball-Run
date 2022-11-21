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
            material.color = new Color(material.color.r,material.color.g,material.color.b,1);
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
