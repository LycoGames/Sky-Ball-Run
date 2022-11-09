using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Game.Runner.Ball
{
    public class Trail : MonoBehaviour
    {
        [SerializeField] private int maxBall=20;
        [SerializeField] private BallManager ballObject;
        [SerializeField] private float waitForRemove = 10f;
        private List<BallManager> ballList=new List<BallManager>();
        private Queue<BallManager> removeQueue=new Queue<BallManager>();
        private WaitForSeconds wfsWaitForRemove;
        private bool isRuning;

        private void Start()
        {
            wfsWaitForRemove = new WaitForSeconds(waitForRemove);
        }
        public void RemoveBallInQueue(BallManager ball)
        {
            removeQueue.Enqueue(ball);
            if (!isRuning)
            {
                isRuning = true;
                StartCoroutine(waitingForRemoveQueue());
            }
        }
        public void AddBall()
        {
            if (ballList.Count >= maxBall) return;
            BallManager createdBall = Instantiate(ballObject);
            ballList.Add(createdBall);
            createdBall.InitializeBall(this);
        }
        public BallManager GetNextBall(BallManager ball)
        {
            int index = ballList.IndexOf(ball);
            if (index > 0) return ballList[index-1];
            return null;
        }
        
        IEnumerator waitingForRemoveQueue()
        {
            yield return wfsWaitForRemove;
            while (removeQueue.Any())
            {
                BallManager ball = removeQueue.Dequeue();
                int index = ballList.IndexOf(ball);
                ballList.Remove(ball);
                ball.RemoveBall();
            }
            isRuning = false;
        }

    }
}
