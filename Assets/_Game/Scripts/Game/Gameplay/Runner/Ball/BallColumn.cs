using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Ball
{
    public class BallColumn : MonoBehaviour
    {
        [SerializeField] private ColumnMover columnMover;
        private Trail trail;
        private int maxBallSize;
        private List<Ball> balls = new List<Ball>();

        public int BallCount() => balls.Count;
        public void InitializeBallColumn(Transform follow,float _distance,int _maxBallSize,Trail _trail)
        {
            trail = _trail;
            maxBallSize = _maxBallSize;
            columnMover.SetFollow(follow);
        }

        public void RegisterColumn(Ball ball)
        {
            if (maxBallSize <= BallCount())
            {
                ball.gameObject.SetActive(false);
                return;
            }
            if(BallCount()==0)trail.IncreaseActiveBallColumnCount();
            balls.Add(ball);
            ball.SetHeight(balls.Count-1);
        }

        public void UnregisterColumn(Ball ball)
        {
            balls.Remove(ball);
            if (BallCount() <= 0)
            {
                trail.DecreaseActiveBallColumnCount();
                return;
            }
            SetHeight();
        }
        public Ball GetBall(int index)
        {
            if (BallCount() > index)
            {
                Ball removedBall=balls[index];
                UnregisterColumn(removedBall);
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




