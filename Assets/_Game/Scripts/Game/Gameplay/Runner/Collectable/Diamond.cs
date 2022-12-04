using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Collectable
{
    public class Diamond : MonoBehaviour
    {
        [SerializeField] private Collider collider;
        [SerializeField] private int value=5;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                collider.enabled = false;
                GameManager.Instance.GainedDiamond(value);
                Destroy(gameObject);
            }
        }
    }
}
