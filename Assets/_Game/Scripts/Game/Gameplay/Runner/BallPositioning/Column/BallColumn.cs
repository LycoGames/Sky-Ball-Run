using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column;
using _Game.Scripts.Game.ObjectPools;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.BallPositioning
{
    public class BallColumn : MonoBehaviour
    {
        [SerializeField] private ColumnMover columnMover;
        [SerializeField] private float distance=0.5f;
        private int maxBallSize;
        private List<Ball> balls = new List<Ball>();

        public int BallCount() => balls.Count;

        public bool CheckIsActive()
        {
            if (balls.Any())
            {
                columnMover.IsFollow = true;
                return true;
            }
            columnMover.IsFollow = false;
            return false;
            
        }
        public void InitializeBallColumn(Transform follow,float _distance,int _maxBallSize)
        {
            maxBallSize = _maxBallSize;
            columnMover.SetFollow(follow);
        }
        public void RegisterColumn(Ball ball)
        {
            if (maxBallSize <= BallCount())
            {
                ball.gameObject.SetActive(false);
                ball.transform.parent = BallPool.ballPool.transform;
                return;
            }
            balls.Add(ball);
            ball.SetHeight(balls.Count-1);
        }

        public void UnregisterColumn(Ball ball)
        {
            RemoveBallList(ball);
            if (BallCount() <= 0) return;
            SetHeight();
        }

        private void RemoveBallList(Ball ball)
        {
            balls.Remove(ball);
            ball.transform.parent = BallPool.ballPool.transform;
        }

        public Ball GetBall(int index)
        {
            if (BallCount() > index)
            {
                Ball removedBall=balls[index];
                RemoveBallList(removedBall);
                return removedBall;
            }
            return null;
        }
        
        public void SetHeight()
        {
            //TODO fazla işlem olabilir.
            float height = 0;
            foreach (Ball ball in balls)
            {
                ball.SetHeight(height);
                height++;
            }
        }


    }
}



