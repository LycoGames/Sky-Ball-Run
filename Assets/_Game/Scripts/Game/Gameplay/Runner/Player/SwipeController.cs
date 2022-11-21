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

        void Start()
        {
            firstTouch = Vector3.zero;
            playerController = GameManager.Instance.GetPlayerController();
        }

        void Update()
        {
            TouchHandler();
            MovePlayer();
        }

        void TouchHandler()
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