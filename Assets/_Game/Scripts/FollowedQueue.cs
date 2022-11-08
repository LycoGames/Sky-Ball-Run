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
        private BallManager ballManager;
        private List<Ball> balls=new List<Ball>();
        private int ballCount;

        public int GetBallCount() => ballCount;
        public void Initiliaze(int maxRow,BallManager ballManager)
        {
            this.ballManager = ballManager;
            this.maxRow = maxRow;
        }

        public void AddBall()
        {
            if (ballCount >= maxRow) return;
            ball = Instantiate(ball);
            if(balls.Any())ball.InitializeBall(this,balls[ballCount-1]);
            else ball.InitializeBall(this,null);
            balls.Add(ball);
            ballCount++;
        }
        
        public void RemoveBall(Ball ball)
        {
            ballCount--;
        }
        
    }
}