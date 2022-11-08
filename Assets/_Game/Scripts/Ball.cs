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

        public Action OnBallLeaveQueue;
        
        [SerializeField] private float rotateSpeed = 2f;
        [SerializeField] private float followDistance = 0.33f;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float changePosSpeed = 0.05f;
        [SerializeField] private float minXPos=-8.75f;
        [SerializeField] private float maxXPos=8.75f;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private ParticleSystem particleSystem;

       
        private FollowedQueue followedQueue;
        private Ball nextBall;
        private Transform follow;
        private float distanceToNextPosX;
        private float distanceToNextPosY;
        private BallState ballState;
        private float removeTime;

        public Ball GetNextBall() => nextBall;
        private void FixedUpdate()
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
                // removeTime += Time.deltaTime;
                // if(removeTime>3)gameObject.SetActive(false);
                return;
            }
        }
        
        public void InitializeBall(FollowedQueue followedQueue, Ball nextBall)
        {
            SetFollowed(followedQueue);
            SetNextBall(nextBall);
            Register();
            SetFollowTransform();
            SetRigidbody();
            SpawnBehindQueue();
            OnFollowChange();
        }

        private void SetNextBall(Ball nextBall)
        {
            this.nextBall = nextBall;
        }

        private void SetFollowed(FollowedQueue followedQueue)
        {
            this.followedQueue = followedQueue;
        }


        private void LeaveQueue()
        {
            OnBallLeaveQueue?.Invoke();
            UnRegister();
            RemoveBallInQueue();
            SetRigidbodyOnLeaveQueue();
            ballState = BallState.NotInQueue;
            SetAnimation();
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
        

        private void SetRotation()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, follow.rotation, rotateSpeed * Time.deltaTime);
        }

        private void SetPosition(float zOffset, float yOffset)
        {
            Vector3 newPos = follow.position - (transform.forward * followDistance);
            newPos.x = Mathf.Clamp(newPos.x , minXPos, maxXPos);
            newPos.y += yOffset;
            newPos.z -= zOffset;
            
            transform.position = newPos;
        }

        public void OnFollowChange()
        {
            Vector3 pos = follow.position - transform.forward*followDistance - transform.position;
            distanceToNextPosX += pos.z;
            distanceToNextPosY += pos.y;
            ballState = BallState.ChangePosition;
        }

        private void ChangeInQueuePosition()
        {
            if (distanceToNextPosX > 0)
            {
                SetPosition(distanceToNextPosX, distanceToNextPosY);
                distanceToNextPosX -= changePosSpeed * Time.deltaTime;
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


        private void SetFollowTransform()
        {
            
            if (nextBall != null) follow = nextBall.transform;
            else follow = followedQueue.transform;
        }

        private void SetRigidbody()
        {
            rb.velocity = new Vector3(0, 0, 0);
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        private void RemoveBallInQueue()
        {
            followedQueue.RemoveBall(this);
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

        private void ChangeNextBall()
        {
            if (nextBall!=null) nextBall = nextBall.nextBall;
            SetFollowTransform();
            OnFollowChange();
        }
        private void SpawnBehindQueue()
        {
            transform.position = follow.position - (transform.forward * 10);
        }
        private void Register()
        {
            if (nextBall != null) nextBall.OnBallLeaveQueue += ChangeNextBall;
        }

        private void UnRegister()
        {
            if (nextBall != null) nextBall.OnBallLeaveQueue -= ChangeNextBall;
        }
    }
}