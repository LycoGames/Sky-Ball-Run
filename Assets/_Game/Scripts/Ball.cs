using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class Ball : MonoBehaviour
    {
        enum BallState
        {
            Follow,
            ChangePosition,
            NotInQueue
        }
        
        [SerializeField] private float rotateSpeed = 2f;
        [SerializeField] private float followDistance = 0.33f;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float changePosSpeed = 0.05f;
        [SerializeField] private float minXPos=-8.75f;
        [SerializeField] private float maxXPos=8.75f;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private ParticleSystem particleSystem;

        public int xPos;
        public int yPos;
        private Followed followed;
        private Transform follow;
        private float distanceToNextPosX;
        private float distanceToNextPosY;
        private BallState ballState;
        private float removeTime;

        public void InitializeBall(Followed followed, int xPos, int yPos)
        {
            SetPos(xPos, yPos);
            SetFollow(followed);
            SetRigidbody();
            Vector3 newPos = follow.position - (transform.forward * followDistance * (xPos + 1));
            distanceToNextPosX = -5;
            SetBallState(BallState.ChangePosition);
        }

        private void SetPos(int xPos, int yPos)
        {
            this.yPos = yPos;
            this.xPos = xPos;
        }

        public void LeaveQueue()
        {
            RemoveBallInQueue();
            SetRigidbodyOnLeaveQueue();
            SetAnimation();
            SetBallState(BallState.NotInQueue);
        }

        private void SetAnimation()
        {
            particleSystem.Play();
            meshRenderer.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (gameObject.CompareTag("Player")) return;
            if (other.CompareTag("Obstacle") && ballState != BallState.NotInQueue)
            {
                LeaveQueue();
            }
        }

        void FixedUpdate()
        {
            if (ballState.Equals(BallState.Follow))
            {
                SetPosition(0, 0);
                SetRotation();
            }
            else if (ballState.Equals(BallState.ChangePosition))
            {
                ChangeInQueuePosition();
                SetRotation();
            }
            else if (ballState.Equals(BallState.NotInQueue))
            {
                removeTime += Time.deltaTime;
                if(removeTime>3)Destroy(gameObject);
                return;
            }
        }

        private void SetRotation()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, follow.rotation,
                Math.Clamp(rotateSpeed / (xPos + 1) * 2, 1, rotateSpeed) * Time.deltaTime);
        }

        private void SetPosition(float zOffset, float yOffset)
        {
            Vector3 newPos = follow.position - (transform.forward * followDistance * (xPos + 1));
            newPos.x = Mathf.Clamp(newPos.x , minXPos, maxXPos);
            newPos.y = follow.position.y + yOffset + (followDistance * yPos) + (transform.rotation.x * xPos);
            newPos.z += zOffset;
            transform.position = newPos;
        }

        public void OnFollowChange(int xPos, int yPos)
        {
            distanceToNextPosX += (xPos-this.xPos ) * followDistance;
            this.xPos = xPos;
            distanceToNextPosY += (yPos-this.yPos) * followDistance;
            this.yPos = yPos;
            ballState = BallState.ChangePosition;
        }

        private void ChangeInQueuePosition()
        {
            if (distanceToNextPosX < 0)
            {
                SetPosition(distanceToNextPosX, distanceToNextPosY);
                distanceToNextPosX += changePosSpeed * Time.deltaTime;
                return;
            }
            distanceToNextPosX = 0;
            if (distanceToNextPosY > 0)
            {
                SetPosition(distanceToNextPosX, distanceToNextPosY);
                distanceToNextPosY -= changePosSpeed * Time.deltaTime;
                return;
            }
            distanceToNextPosY = 0;
            SetBallState(BallState.Follow);
        }


        private void SetFollow(Followed followed)
        {
            this.followed = followed;
            follow = followed.transform;
        }

        private void SetRigidbody()
        {
            rb.velocity = new Vector3(0, 0, 0);
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        private void RemoveBallInQueue()
        {
            followed.RemoveBall(this);
        }

        private void SetBallState(BallState ballState)
        {
            this.ballState = ballState;
        }

        private void SetRigidbodyOnLeaveQueue()
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.velocity = new Vector3(Random.Range(-10, 10), 10, -3);
        }
    }
}