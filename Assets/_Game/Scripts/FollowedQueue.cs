using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts
{
    public class FollowedQueue : MonoBehaviour
    {
        private int maxRow;
        [SerializeField]private Ball ball;
        [SerializeField] private float waitForRepositioningQueue;
        private FollowedQueue nextFollowedQueue;
        private BallManager ballManager;
        private List<Ball> balls=new List<Ball>();
        private Queue<IEnumerator> leaveBallProcess=new Queue<IEnumerator>();
        private int ballCount;
        private Coroutine waitForRepositioning;
        private WaitForSeconds wfsPositioning; 
        

        public int GetBallCount() => ballCount;

        private void FixedUpdate()
        {
            foreach (Ball ball in balls)
            {
                ball.BallUpdate();
            }
        }

        public void AddLeaveBallProcessToQueue(IEnumerator process)
        {
            // leaveBallProcess.Enqueue(process);
            // if (isStart) isStart = false;
            // else StartCoroutine(WaitingStartProcessHandler());
        }

        public void Initiliaze(int maxRow,BallManager ballManager,FollowedQueue nextFollowedQueue)
        {
            this.nextFollowedQueue = nextFollowedQueue;
            this.ballManager = ballManager;
            this.maxRow = maxRow;
            wfsPositioning = new WaitForSeconds(waitForRepositioningQueue);
        }

        public Ball GetBall()
        {
            Ball ball = balls[ballCount-1];
            RemoveBall(ball);
            return ball;
        }

        public void AddBall()
        {
            if (ballCount >= maxRow) return;
            ball = Instantiate(ball);
            ball.InitializeBall(this,GetLastBallInQueue());
            balls.Add(ball);
            ballCount++;
        }
        
        public void RemoveBall(Ball ball)
        {
            ballCount--;
            balls.Remove(ball);
            leaveBallProcess.Enqueue(TakeBallFromTop());
        }

        private bool isStart=false;
        private IEnumerator WaitingStartProcessHandler()
        {
            while (!isStart)
            {
                isStart = true;
                yield return wfsPositioning;
            }

            StartCoroutine(ProcessHandler());
        }
        
        private IEnumerator ProcessHandler()
        {
            while(leaveBallProcess.Any())
            {
                yield return StartCoroutine(leaveBallProcess.Dequeue());
            }
            yield return null;
        }

        private Ball GetLastBallInQueue()
        {
            Ball ball;
            if (ballCount > 0) return balls[ballCount-1];
            else return null;
        }
        IEnumerator TakeBallFromTop()
        {
            if (nextFollowedQueue == null) yield return null;
            if (ballCount >= nextFollowedQueue.GetBallCount()) yield return null;
            Ball ball = nextFollowedQueue.GetBall();
            balls.Add(ball);
            ballCount++;
            yield return StartCoroutine(ball.ChangeQueue(this,GetLastBallInQueue()));
        }
    }
}