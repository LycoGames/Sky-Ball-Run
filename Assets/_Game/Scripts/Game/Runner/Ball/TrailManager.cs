using UnityEngine;

namespace _Game.Scripts.Game.Runner.Ball
{
    public class TrailManager : MonoBehaviour
    {
        [SerializeField] private Trail trail;
        [SerializeField] private int ballSpawnCount=10;
        private float xPos = -1f;
        void Start()
        {
            for(int i=0;i<5;i++){
            Trail createdTrail = Instantiate(trail, transform);
            createdTrail.transform.localPosition = new Vector3(xPos, 0.01f, 0);
            xPos -= 0.5f;
            for(int j=0;j<ballSpawnCount;j++)createdTrail.AddBall();
            }
        }

        
    }
}
