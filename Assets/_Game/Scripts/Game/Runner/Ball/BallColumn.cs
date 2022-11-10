using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Game.Runner.Ball
{
    public class BallColumn : MonoBehaviour
    {
        [SerializeField] private ColumnMover columnMover;
        [SerializeField] private float waitForRequestingBallFromBack=1f;
        public int BallCount { get; private set; }

        private float distance;
        private Trail trail;
        private List<Ball> balls = new List<Ball>();
        private bool isStartRequestBallCoroutine;

        public void InitializeBallColumn(Transform follow, Trail _trail, float _distance)
        {
            trail = _trail;
            columnMover.SetFollow(follow);
            distance = _distance;
        }

        public void AddBall(Ball ball)
        {
            if (ball == null) return;
            columnMover.isFollow = true;
            BallCount++;
            balls.Add(ball);
            ball.transform.parent = transform;
            ball.SetBall(this, (balls.Count - 1) * distance);
        }

        public void RemoveBall(Ball ball)
        {
            RemoveBallInList(ball);
            DisablingMover();
            SetHeight();
            Invoke("RequestingBall",waitForRequestingBallFromBack);
        }

        public Ball GetBall(int index)
        {
            if (index > BallCount) return null;
            Ball ball = balls[index];
            RemoveBallInList(ball);
            if (BallCount >= index) SetHeight();
            DisablingMover();
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

        private void RequestingBall()
        {
            //Invoke("StartRequestingBall",waitForRequestingBallFromBack);
            trail.RepositioningToForward(BallCount+1);
        }

        
       
        private void RemoveBallInList(Ball ball)
        {
            balls.Remove(ball);
            BallCount--;
        }

        private void DisablingMover()
        {
            if (!balls.Any()) columnMover.isFollow = false;
        }
    }
}




// private void StartRequestingBall()
// {
//     if (isStartRequestBallCoroutine) return;
//     isStartRequestBallCoroutine = true;
//     StartCoroutine(RequestBallCoroutine());
// }



// private IEnumerator RequestBallCoroutine()
// {
//     while (trail.CheckForLongerColumn(this))
//     {
//         BallColumn backColumn = trail.GetBackColumn(this);
//         if (BallCount < backColumn.BallCount)
//         {
//             Ball ball = backColumn.GetBall(BallCount);
//             AddBall(ball);
//         }
//
//         yield return null;
//     }
//
//     isStartRequestBallCoroutine = false;
//     yield return null;
// }
