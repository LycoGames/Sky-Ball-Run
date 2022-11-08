using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts
{
    public class FollowedQueue : MonoBehaviour
    {
        private int maxRow;
        [SerializeField]private Ball ball;
        private FollowedQueue nextFollowedQueue;
        private BallManager ballManager;
        private List<Ball> balls=new List<Ball>();
        private int ballCount;

        public int GetBallCount() => ballCount;
        public void Initiliaze(int maxRow,BallManager ballManager,FollowedQueue nextFollowedQueue)
        {
            this.nextFollowedQueue = nextFollowedQueue;
            this.ballManager = ballManager;
            this.maxRow = maxRow;
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
            TakeBallFromTop();
        }

        private Ball GetLastBallInQueue()
        {
            Ball ball;
            if (ballCount > 0) return balls[ballCount-1];
            else return null;
        }
        private void TakeBallFromTop()
        {
            if (nextFollowedQueue == null) return;
            if (ballCount >= nextFollowedQueue.GetBallCount()) return;
            Ball ball = nextFollowedQueue.GetBall();
            ball.ChangeQueue(this,GetLastBallInQueue());
            balls.Add(ball);
            ballCount++;
        }
    }
}