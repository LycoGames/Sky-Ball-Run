using System;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Obstacles.OpenCloseBarricade
{
    public class OpenCloseBarricade : MonoBehaviour
    {
        [SerializeField] private Collider myCollider;
        [SerializeField] private Transform leftBarricade;
        [SerializeField] private Transform rightBarricade;
        [SerializeField] private float barricadeOpenTime=2f;
        private bool isFirstTouch = true;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball")&&isFirstTouch)
            {
                isFirstTouch = false;
                GameManager.Instance.OnRevive += OpenBarricade;
            }
        }

        public void OnDisable()
        {
            GameManager.Instance.OnRevive -= OpenBarricade;
        }

        public void OnButtonPress()
        {
            myCollider.enabled = false;
            OpenBarricade();
        }

        private void OpenBarricade()
        {
            leftBarricade.DOLocalRotate(new Vector3(90, 0, 0), barricadeOpenTime);
            rightBarricade.DOLocalRotate(new Vector3(90, 0, 0), barricadeOpenTime);
        }
    }
}
