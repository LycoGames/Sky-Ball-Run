using System;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class ScaleRemoverGate : MonoBehaviour
    {
        [SerializeField] private AdderGateSpecs selectedGate;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private float xPos;
        [SerializeField] private TextMeshProUGUI sizeText;

        private void Start()
        {
            Vector3 newPos = transform.position;
            newPos.x = xPos;
            transform.position = newPos;
            sizeText.text = "-" + selectedGate.addSize;
        }

        private void OnTriggerEnter(Collider other)
        {
        
            if (other.CompareTag("Ball"))
            {
                boxCollider.enabled = false;
                switch (selectedGate.adderType)
                {
                    case AdderType.RightRemover:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.RightRemover(selectedGate.addSize));
                        break;
                    case AdderType.UpRemover:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.UpRemover(selectedGate.addSize));
                        break;
                    case AdderType.LengthRemover:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.LengthRemover(selectedGate.addSize));
                        break;
                }
                gameObject.SetActive(false);
            }
        }
        [Serializable]
        public struct AdderGateSpecs
        {
            public AdderType adderType;
            public int addSize;
        }

        public enum AdderType
        {
            RightRemover,
            UpRemover,
            LengthRemover
        }
    }
}
