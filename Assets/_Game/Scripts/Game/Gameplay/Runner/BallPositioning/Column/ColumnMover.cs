using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column
{
    public class ColumnMover : MonoBehaviour
    {
        private Transform follow;
        [SerializeField] private float distance = 0.5f;
        [SerializeField] private float rotateSpeed = 5;
        [SerializeField] private float minXPos = 8;
        [SerializeField] private float maxXPos = 8;
        public bool IsFollow { private get; set; }

        public void SetFollow(Transform _follow)
        {
            follow = _follow;
        }

        void FixedUpdate()
        {
            //if (!IsFollow) return;
            SetPosition();
            SetRotation();
        }

        private void SetRotation()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, follow.rotation, rotateSpeed * Time.deltaTime);
        }

        private void SetPosition()
        {
            Vector3 newPos = follow.position - (follow.forward * distance);
            newPos.x = Mathf.Clamp(newPos.x, minXPos, maxXPos);
            newPos.y = Mathf.Clamp(newPos.y, -50f, 100);
            transform.position = newPos;
        }
    }
}