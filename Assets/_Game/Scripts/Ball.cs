using _Game.Scripts.Player;
using UnityEngine;

namespace _Game.Scripts
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Transform follow;
        [SerializeField] private float followSpeed=10f;
        [SerializeField] private Rigidbody _theRb;

        public void InitializeBall(Transform follow)
        {
            this.follow = follow;
        }
        void Update()
        {
            _theRb.MoveRotation(follow.rotation);
            Vector3 newPos = follow.position - (transform.forward /followSpeed);
            newPos.x = Mathf.Clamp(newPos.x, -8.5f, 8.5f);
            _theRb.MovePosition(newPos);
        }
    }
}
