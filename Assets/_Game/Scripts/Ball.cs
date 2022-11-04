using System;
using _Game.Scripts.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class Ball : MonoBehaviour
    {
        private Followed followed;
        public Ball followBall;
        public Ball followedBall;
        [SerializeField] private float rotateSpeed = 2f;
        [SerializeField] private float followDistance = 0.33f;
        [SerializeField] private Rigidbody rb;
        private bool isFollow = true;

        void FixedUpdate()
        {
            if (isFollow)
            {
                SetPosition();
                SetRotation();
            }
        }
        public void InitializeBall(Ball ballFollow, Followed followed)
        {
            this.followBall = ballFollow;
            ballFollow.followedBall = this;
            this.followed = followed;//
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                isFollow = false;
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.velocity = new Vector3(Random.Range(-3, 3), 5f, -5f)*5f;
                followed.RemoveBall(this);
                if (followedBall == null) return;
                followedBall.followBall = followBall;
                followBall.followedBall = followedBall;
            } //TODO daha temiz yazılabilir.
        }

        private void SetRotation()
        {
            transform.rotation =
                Quaternion.Lerp(transform.rotation, followBall.transform.rotation, rotateSpeed * Time.deltaTime);
        }

        private void SetPosition()
        {
            Vector3 newPos = followBall.transform.position - (followBall.transform.forward * followDistance);
            newPos.x = Mathf.Clamp(newPos.x, -8.5f, 8.5f);
            transform.position = newPos;
        }
    }
}