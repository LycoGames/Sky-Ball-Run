using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class ScaleRemoverGate : MonoBehaviour
    {
        [SerializeField] private AdderGateSpecs selectedGate;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private float xPos;
        [SerializeField] private TextMeshProUGUI sizeText;
        [SerializeField] private float checkTime = 2f;
        private int removeSize=1;
        private BallManager ballManager;
        private WaitForSeconds wfsForCheckSize;

        private void OnEnable()
        {
            ballManager=BallManager.Instance;
            StartCoroutine(CheckSize());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void Start()
        {
            wfsForCheckSize = new WaitForSeconds(checkTime);
            Vector3 newPos = transform.position;
            newPos.x = xPos;
            transform.position = newPos;
            sizeText.text = "-" + removeSize;
        }

        private void OnTriggerEnter(Collider other)
        {
        
            if (other.CompareTag("Ball"))
            {
                boxCollider.enabled = false;
                switch (selectedGate.adderType)
                {
                    case AdderType.RightRemover:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.RightRemover(removeSize));
                        break;
                    case AdderType.UpRemover:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.UpRemover(removeSize));
                        break;
                    case AdderType.LengthRemover:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.LengthRemover(removeSize));
                        break;
                }
                gameObject.SetActive(false);
            }
        }
        
        private IEnumerator CheckSize()
        {
            float newRemoveSize=0;
            while (true)
            {
                
                switch (selectedGate.adderType)
                {
                    case AdderType.RightRemover:
                        newRemoveSize = ballManager.currentColumn * ((float)selectedGate.removePercentage/100);
                        break;
                    case AdderType.UpRemover:
                        newRemoveSize = ballManager.currentFloor * ((float)selectedGate.removePercentage/100);
                        break;
                    case AdderType.LengthRemover:
                        newRemoveSize = ballManager.currentRow * ((float)selectedGate.removePercentage/100);
                        break;
                }

                removeSize=(int)Math.Round(newRemoveSize);
                if (removeSize <= 0) removeSize=1;
                sizeText.text = "-" + removeSize;
                yield return wfsForCheckSize;
            }
        }
        [Serializable]
        public struct AdderGateSpecs
        {
            public AdderType adderType;
            public int removePercentage;
        }

        public enum AdderType
        {
            RightRemover,
            UpRemover,
            LengthRemover
        }
    }
}