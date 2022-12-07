using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.BallPositioning
{
    public class BallCountCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ballCountText;
        [SerializeField] private float distance = 1f;
        private Transform mainCam;
        private BallManager ballManager;
        private Vector3 newPos;

        private void OnEnable()
        {
            ballManager=BallManager.Instance;
            ballManager.OnTotalBallCountChange += OnBallCountChange;
            ballManager.OnShapeChange+=SetHeight;
            mainCam = Camera.main.transform;
        }

        private void OnDisable()
        {
            if(BallManager.Instance!=null)BallManager.Instance.OnTotalBallCountChange -= OnBallCountChange;
        }

        private void FixedUpdate()
        {
            transform.LookAt(mainCam);
        }

        private void OnBallCountChange(int value)
        {
            ballCountText.text = value.ToString();
            SetHeight();
            
        }

        private void SetHeight()
        {
            int floor = ballManager.currentFloor;
            //TODO Kritik hata olasılığı.
            if (floor <= 0)
            {
                Invoke("SetHeight",.25f);
                return;
            }
            newPos = transform.position;
            newPos.y = (floor + 1) * distance;
            transform.position = newPos;
        }
    }
}
