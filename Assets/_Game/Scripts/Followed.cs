using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts
{
    public class Followed : Ball
    {
        private Floor floor;
        private List<Ball> balls=new List<Ball>();
        public void Initialize(Floor floor) => this.floor=floor;
        
        public int GetBallCount() => balls.Count;

        private void Awake()
        {
        }

        private void FixedUpdate()
        {
            
        }

        public void AddBall()
        {
            Ball createdBall = Instantiate(BallManager.ballManager.ball);
            if(balls.Count>0)createdBall.InitializeBall(balls[balls.Count-1],this);
            else createdBall.InitializeBall(this,this);
            balls.Add(createdBall);
        }

        public void RemoveBall(Ball ball)
        {
            BallManager.ballManager.RemoveBall();
            balls.Remove(ball);
            if (balls.Count<=0) floor.DisableFollowed(this);
        }
    }
}
