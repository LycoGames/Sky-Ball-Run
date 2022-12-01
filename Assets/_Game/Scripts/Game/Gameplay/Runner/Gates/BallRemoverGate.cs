using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class BallRemoverGate : MonoBehaviour
    {
        [SerializeField] private Collider collider;
        [SerializeField] private int removePercentage;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float checkTime = 2f;
        private int removeSize = 1;
        private BallManager ballManager;
        private WaitForSeconds wfsForCheckSize;

        private void OnEnable()
        {
            wfsForCheckSize = new WaitForSeconds(checkTime);
            ballManager = BallManager.Instance;
            StartCoroutine(CheckSize());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                collider.enabled = false;
                BallManager.Instance.StartCoroutine(BallManager.Instance.RemoveBall(removeSize));
                gameObject.SetActive(false);
            }
        }
        private IEnumerator CheckSize()
        {
            float newRemoveSize = 0;
            while (true)
            {
                newRemoveSize = ballManager.TotalBallCount * ((float)removePercentage / 100);
                removeSize = (int)Math.Round(newRemoveSize);
                if (removeSize <= 0) removeSize = 1;
                text.text = "-" + removeSize;
                yield return wfsForCheckSize;
            }
        }
    }
}