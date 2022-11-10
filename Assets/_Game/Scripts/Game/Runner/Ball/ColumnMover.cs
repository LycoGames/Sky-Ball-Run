using UnityEngine;

namespace _Game.Scripts.Game.Runner.Ball
{
    public class ColumnMover : MonoBehaviour
    {
        private Transform follow;
        [SerializeField] private float distance = 0.5f;
        [SerializeField] private float rotateSpeed = 5;
        [SerializeField] private float minXPos = 8;
        [SerializeField] private float maxXPos = 8;

        public void SetFollow(Transform _follow)
        {
            follow = _follow;
        }

        void FixedUpdate()
        {
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
            newPos.y = Mathf.Clamp(newPos.y, 0.01f, 100);
            transform.position = newPos;
        }
    }
}