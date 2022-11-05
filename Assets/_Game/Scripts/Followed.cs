using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Player;
using UnityEngine;

namespace _Game.Scripts
{
    public class Followed : MonoBehaviour
    {
        private Floor floor;
        [SerializeField] private int createdBall=1;
        private List<Ball> balls=new List<Ball>();
        [SerializeField] private Ball ball;
        public void Initialize(Floor floor) => this.floor=floor;
        
        public int GetBallCount() => balls.Count;

        private void Start()
        {
            if (createdBall <= 1) createdBall = 1;
            for(int i=0;i<createdBall;i++)AddBall();
        }

        private void FixedUpdate()
        {
            
        }

        public void RemoveBallIndex(int ballIndex)
        {
            if (balls.Count > ballIndex)
            {
                balls[ballIndex].LeaveQueue();
            }
        }
        public void AddBall()
        {
            Ball createdBall;
            createdBall=Instantiate(ball);
            if (!balls.Any())
            {
                createdBall.InitializeBall(null,this);
            }
            else createdBall.InitializeBall(balls[balls.Count-1],this);
            balls.Add(createdBall);
            // Ball createdBall = Instantiate(BallManager.ballManager.ball);
            // if(balls.Count>0)createdBall.InitializeBall(balls[balls.Count-1],this);
            // else createdBall.InitializeBall(this,this);
            // balls.Add(createdBall);
        }

        public void RemoveBall(Ball ball)
        {
            balls.Remove(ball);
        }

        private void OnTriggerEnter(Collider other)
        {
            print("calisti");
        }
    }
}
