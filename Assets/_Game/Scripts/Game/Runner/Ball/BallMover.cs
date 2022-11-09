using UnityEngine;

namespace _Game.Scripts.Game.Runner.Ball
{
    public class BallMover : MonoBehaviour
    {
        private Transform follow;
        [SerializeField] private BallManager ballManager;
        [SerializeField] private float distance = 0.5f;
        [SerializeField] private float followCatchSpeed = 2f;
        [SerializeField] private float rotateSpeed = 5;
        [SerializeField] private float minXPos = 8;
        [SerializeField] private float maxXPos = 8;
        private float zOffset;
        private float yOffset;
        private bool isFollow;

        public void SetOffset()
        {
            Vector3 newPos = follow.position - (follow.forward * distance);
            zOffset = newPos.z - transform.position.z;
            yOffset = transform.position.y - newPos.y;
        }

        public void SetFollow(Transform _follow)
        {
            follow = _follow;
        }

        void FixedUpdate()
        {
            SetPosition();
            SetRotation();
            OffsetHandler();
        }

        private void SetRotation()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, follow.rotation, rotateSpeed * Time.deltaTime);
        }

        private void SetPosition()
        {
            Vector3 newPos = follow.position - (follow.forward * distance);
            newPos.x = Mathf.Clamp(newPos.x, minXPos, maxXPos);
            newPos.y = Mathf.Clamp(newPos.y + yOffset, 0.01f, 100);
            newPos.z -= zOffset;
            transform.position = newPos;
        }

        private void OffsetHandler()
        {
            if (zOffset == 0 && yOffset == 0) return;

            if (zOffset > 0)
            {
                zOffset -= followCatchSpeed * Time.deltaTime/2;
                return;
            }

            zOffset = 0;
            if (yOffset > 0)
            {
                yOffset -= followCatchSpeed * Time.deltaTime;
                return;
            }

            yOffset = 0;
        }
    }
}