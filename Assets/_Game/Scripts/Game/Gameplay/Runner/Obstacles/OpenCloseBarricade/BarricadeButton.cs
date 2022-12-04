using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Obstacles.OpenCloseBarricade
{
    public class BarricadeButton : MonoBehaviour
    {
        [SerializeField] private Collider collider;
        [SerializeField] private float pressTime = 0.5f;
        [SerializeField] private OpenCloseBarricade openCloseBarricade;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                collider.enabled = false;
                transform.DOLocalMove(Vector3.zero, pressTime);
                openCloseBarricade.OnButtonPress();
            }
        }
    }
}
