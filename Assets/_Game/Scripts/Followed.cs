using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts
{
    public class Followed : MonoBehaviour
    {
        
        private int maxColumn;
        private int maxRow;
        [SerializeField]private Ball ball;
        private BallManager ballManager;
        
        private Dictionary<int, List<Ball>> ballList=new Dictionary<int, List<Ball>>();
        private int ballCount;

        public int GetBallCount() => ballCount;
        public void Initiliaze(int maxColumn,int maxRow,BallManager ballManager)
        {
            this.ballManager = ballManager;
            this.maxColumn = maxColumn;
            this.maxRow = maxRow;
            InitializeBallList();
        }

        private void InitializeBallList()
        {
            for (int i = ballList.Count; i < maxColumn; i++) ballList.Add(i, new List<Ball>());
        }
        
        

        public void AddBall()
        {
            if (ballCount >= maxColumn * maxRow) return;
            Ball createdBall;
            createdBall = Instantiate(ball);
            createdBall.InitializeBall(this, ballCount%maxRow,ballCount/maxRow);
            ballList[ballCount/maxRow].Add(createdBall);
            ballCount++;
        }
        
        public void RemoveBall(Ball ball)
        {
            ballCount--;
            if (ballCount == 0)
            {
                ballManager.RemoveFollowed(this);
                return;
            }
            PositioningRow(ball);
            //PositioningColumn(ball.yPos,ball.xPos);
        }

      
        private void PositioningColumn(int column,int rowPos)
        {
            if(column>=maxColumn-1)return;
            if (ballList[column + 1].Count >= rowPos+1)
            {
                Ball ball = ballList[column + 1][rowPos];
                PositioningRow(ball);
                ball.OnFollowChange(column,rowPos);
                PositioningColumn(column+1,rowPos);
            }
        }

        private void PositioningRow(Ball ball)
        {
            ballList[ball.yPos].Remove(ball);
            for (int i = ball.xPos; i < ballList[ball.yPos].Count; i++)
            {
                ballList[ball.yPos][i].OnFollowChange(i, ball.yPos);
            }
        }
    }
}