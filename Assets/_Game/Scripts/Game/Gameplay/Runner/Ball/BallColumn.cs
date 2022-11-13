using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Ball
{
    public class BallColumn : MonoBehaviour
    {
        [SerializeField] private ColumnMover columnMover;
        [SerializeField] private float waitForRequestingBallFromBack=1f;
        public int BallCount { get; private set; }

        private Queue<Ball> removeQueue=new Queue<Ball>();
        private float distance;
        private Trail trail;
        private List<Ball> balls = new List<Ball>();
        private bool isStartRequestBallCoroutine;

        public int ReturnBallCount() => balls.Count;
        public void InitializeBallColumn(Transform follow, Trail _trail, float _distance)
        {
            trail = _trail;
            columnMover.SetFollow(follow);
            distance = _distance;
        }

        public void AddBall(Ball ball)
        {
            if (ball == null) return;
            BallCount++;
            balls.Add(ball);
            ball.transform.parent = transform;
            ball.SetBall(this, (balls.Count - 1) * distance);
        }

        public void RemoveBall(Ball ball)
        {
            removeQueue.Enqueue(ball);
        }

        public void StartRemoveProcess()
        {
            while (removeQueue.Any())
            {
                Ball ball = removeQueue.Dequeue();
                RemoveBallInList(ball);
                SetHeight();
                ball.gameObject.SetActive(false);
            }
            
        }

        public Ball GetBall(int index)
        {
            if(index >= balls.Count||index<0)
            {
                Debug.Break();
                print(index);
            }
            Ball ball = balls[index];
            RemoveBallInList(ball);
            return ball;
        }
        
        private void SetHeight()
        {
            //TODO fazla işlem olabilir.
            float height = 0;
            foreach (Ball ball in balls)
            {
                ball.SetHeight(height);
                height += distance;
            }
        }

        private void RemoveBallInList(Ball ball)
        {
            balls.Remove(ball);
            BallCount--;
        }

      
    }
}




