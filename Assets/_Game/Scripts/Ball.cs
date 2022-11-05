using System;
using _Game.Scripts.Player;
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
        public Action onBallLeaveQueue;
        public Ball nextBall;
        
        [SerializeField] private float rotateSpeed = 2f;
        [SerializeField] private float followDistance = 0.33f;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float changePosSpeed=0.05f;
        
        private Followed followed;
        private Transform follow;
        private float distanceToNextPos;
        private BallState ballState;
       
        public void InitializeBall(Ball followBall,Followed followed)
        {
            this.followed = followed;
            this.nextBall = followBall;
            SetFollow();
            SetRigidbody();
            SetBallState(BallState.Follow);
            Register();
        }
        public void LeaveQueue()
        {
            RemoveBallInQueue();
            Unregister();
            onBallLeaveQueue?.Invoke();
            SetRigidbodyOnLeaveQueue();
            SetBallState(BallState.NotInQueue);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (gameObject.CompareTag("Player")) return;
            if (other.CompareTag("Obstacle")&&ballState==BallState.Follow)
            {
                LeaveQueue();
            } 
        }

        void FixedUpdate()
        {
            
            if (ballState.Equals(BallState.Follow))
            {
                SetPosition();
                SetRotation();
            }
            else if(ballState.Equals(BallState.ChangePosition))
            {
                ChangeInQueuePosition();
                SetRotation();
            }
            else if (ballState.Equals(BallState.NotInQueue))
            {
                return;
            }
        }
        private void SetRotation()
        {
            transform.rotation =
                Quaternion.Lerp(transform.rotation, follow.rotation, rotateSpeed * Time.deltaTime);
        }

        private void SetPosition()
        {
            Vector3 newPos = follow.position - (follow.forward * followDistance);
            newPos.x = Mathf.Clamp(newPos.x, -8.5f, 8.5f);
            transform.position = newPos;
        }

        private void OnFollowChange()
        {
            nextBall = nextBall.nextBall;
            SetFollow();
            distanceToNextPos =  follow.position.z - transform.position.z;
            ballState = BallState.ChangePosition;
        }
        
        private void ChangeInQueuePosition()
        {
            Vector3 newPos = follow.position - (follow.forward * followDistance);
            newPos.x = Mathf.Clamp(newPos.x, -8.5f, 8.5f);
            newPos.z -= distanceToNextPos;
            distanceToNextPos -= changePosSpeed * Time.deltaTime;
            transform.position = newPos;
            if (distanceToNextPos <= 0)
            {
                distanceToNextPos = 0;
                SetBallState(BallState.Follow);
            }
        }
        

        private void SetFollow()
        {
            if (nextBall != null) follow = nextBall.transform;
            else follow = followed.transform;
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

        private void Unregister()
        {
            if(nextBall!=null)nextBall.onBallLeaveQueue -= OnFollowChange;
        }
        private void Register()
        {
            if(nextBall!=null)nextBall.onBallLeaveQueue += OnFollowChange;
        }

        private void SetBallState(BallState ballState)
        {
            this.ballState = ballState;
        }

        private void SetRigidbodyOnLeaveQueue()
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.velocity = new Vector3(Random.Range(-10,10), 10,-3 );
        }
        
    }
}