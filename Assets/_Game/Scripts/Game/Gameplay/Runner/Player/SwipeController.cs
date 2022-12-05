using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Player
{
    public class SwipeController : MonoBehaviour
    {
        [SerializeField] private float sense = 5f;
        private bool isStillTouch = false;
        private Vector3 firstTouch;
        private PlayerController playerController;
        private float slipOnX = 0;
        private bool canRotate;

        private void Start()
        {
            firstTouch = Vector3.zero;

        }

        private void FixedUpdate()
        {
            if (playerController == null) return;
            if (canRotate) TouchHandler();
        }

        public void InitiliazeController(PlayerController _playerController) => playerController = _playerController;
        public void StartRotate() => canRotate = true;
        public void StopRotate() => canRotate = false;

        private void TouchHandler()
        {
            if (Input.GetMouseButton(0))
            {
                if (isStillTouch)
                {
                    playerController.SetXPosition(CalculateSliping().x * sense * Time.deltaTime);
                    firstTouch = Input.mousePosition;
                }
                else
                {
                    firstTouch = Input.mousePosition;
                    isStillTouch = true;
                }

                return;
            }

            isStillTouch = false;
            firstTouch = Vector3.zero;
        }

        private Vector3 CalculateSliping()
        {
            return new Vector3(firstTouch.x, firstTouch.y, 0)
                   - Input.mousePosition;
        }
    }
}