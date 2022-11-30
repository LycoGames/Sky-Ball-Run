using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class BallRemoverGate : MonoBehaviour
    {
        [SerializeField] private Collider collider;
        [SerializeField] private int ballCount;
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            text.text = "-"+ballCount;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                collider.enabled = false;
                BallManager.Instance.RemoveBall(ballCount);
                gameObject.SetActive(false);
            }
        }
    }
}