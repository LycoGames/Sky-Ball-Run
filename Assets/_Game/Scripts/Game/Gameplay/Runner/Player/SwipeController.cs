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
        
        private void Update()
        {
            if (playerController == null) return;
            if(canRotate) TouchHandler();
            MovePlayer();
            
        }

        public void InitiliazeController(PlayerController _playerController) => playerController = _playerController;
        public void StartRotate() => canRotate = true;
        public void StopRotate()
        {
            canRotate = false;
            playerController.HorizontalInput = 0;
        }

        private void TouchHandler()
        {
            if (Input.GetMouseButton(0))
            {
                if (isStillTouch) slipOnX = CalculateSliping().x;
                else
                {
                    firstTouch = Input.mousePosition;
                    isStillTouch = true;
                }

                return;
            }

            isStillTouch = false;
            firstTouch = Vector3.zero;
            slipOnX = 0;
        }

        private Vector3 CalculateSliping()
        {
            return new Vector3(firstTouch.x, firstTouch.y, 0)
                   - Input.mousePosition;
        }

        private void MovePlayer()
        {
            playerController.HorizontalInput = Mathf.Clamp(-slipOnX * sense, -1, 1);
        }
    }
}