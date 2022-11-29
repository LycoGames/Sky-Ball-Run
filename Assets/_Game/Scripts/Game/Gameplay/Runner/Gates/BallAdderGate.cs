using System;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class BallAdderGate : MonoBehaviour
    {
        [SerializeField] private Collider collider;
        [SerializeField] private int ballCount;
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            text.text = ballCount.ToString();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                collider.enabled = false;
                BallManager.Instance.AddBall(ballCount);
                gameObject.SetActive(false);
            }
        }
    }
}
