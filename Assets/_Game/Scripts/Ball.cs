using _Game.Scripts.Player;
using UnityEngine;

namespace _Game.Scripts
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Transform follow;
        [SerializeField] private float rotateSpeed=2f;

        void FixedUpdate()
        {
            SetPosition();
        }
        public void InitializeBall(Transform follow)
        {
            this.follow = follow;
        }
        private void SetPosition()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, follow.rotation, rotateSpeed * Time.deltaTime);
            Vector3 newPos = follow.position - (follow.forward / 2);
            newPos.x = Mathf.Clamp(newPos.x, -8.5f, 8.5f);
            transform.position = newPos;
        }
    }
}
