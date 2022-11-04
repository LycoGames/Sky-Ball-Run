using UnityEngine;

namespace _Game.Scripts
{
    public class BallQueueSpawner : MonoBehaviour
    {
        [SerializeField] private Ball ball;
        [SerializeField] private int ballCount;
        [SerializeField] private Transform follow;
        void Start()
        {
            for (int i = 0; i < ballCount; i++)
            {
                Ball createdBall=Instantiate(ball);
               // createdBall.InitializeBall(follow);
                follow = createdBall.transform;
            }
        }

        
    }
}
