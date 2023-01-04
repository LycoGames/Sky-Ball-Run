using System;
using _Game.Scripts.Game.Gameplay.Runner;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Waterfall
{
    public class WaterfallBasket : MonoBehaviour
    {
        [SerializeField] private int pointMultiplier;
        [SerializeField] private TMP_Text multiplierText;

        public DiamondRewardVisualizer DiamondRewardVisualizer { get; set; }

        private void OnEnable()
        {
            multiplierText.text = "X" + pointMultiplier;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Ball ball)) return;

            // ball.ReturnToPool();
            DiamondRewardVisualizer.DiamondRewardSequence(other.transform.position, pointMultiplier);
            other.GetComponent<MeshRenderer>().enabled = false;
            other.GetComponent<AudioSource>().Play();
        }
    }
}