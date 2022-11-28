using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.ObjectPools;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column
{
    public class BallColumn : MonoBehaviour
    {
        [SerializeField] private ColumnMover columnMover;
        private int maxBallSize;
        [SerializeField] private List<Ball> balls = new List<Ball>();

        public int BallCount() => balls.Count;
        public void ClearColumn() => balls.Clear();

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

        public void InitializeBallColumn(Transform follow, int _maxBallSize)
        {
            BallManager.Instance.CheckingCurrentFloor += CheckFloor;
            maxBallSize = _maxBallSize;
            columnMover.SetFollow(follow);
        }

        public void RegisterColumn(Ball ball)
        {
            if (maxBallSize <= BallCount())
            {
                ball.gameObject.SetActive(false);
                ball.transform.parent = BallPool.Instance.transform;
                return;
            }

            balls.Add(ball);
            CheckFloor();
            ball.SetHeight(balls.Count - 1);
        }

        public void UnregisterColumn(Ball ball)
        {
            if (!balls.Contains(ball)) return;
            RemoveBallList(ball);
            if (BallCount() <= 0) return;
            SetHeight();
        }

        public Ball GetBallReference(int index)
        {
            if (BallCount() > index) return balls[index];
            return null;
        }

        public Ball GetBall(int index)
        {
            if (BallCount() > index)
            {
                Ball removedBall = balls[index];
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

        private void RemoveBallList(Ball ball)
        {
            balls.Remove(ball);
            ball.transform.parent = BallPool.Instance.transform;
        }

        private void CheckFloor()
        {
            if (BallManager.Instance.currentFloor < balls.Count)
                BallManager.Instance.currentFloor = balls.Count;
        }
    }
}