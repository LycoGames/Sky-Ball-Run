using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.ObjectPools;
using _Game.Scripts.Game.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Game.Scripts.Game.Gameplay.Runner.Ball
{
    public class BallManager : MonoBehaviour
    {
        static public BallManager ballManager;
        [SerializeField] private PlayerRunner playerRunner;
        [SerializeField] private BallColumn ballColumn;
        [SerializeField] private float distance = 0.5f;
        [SerializeField] private int maxRow = 30;
        [SerializeField] private int maxColumn = 38;
        [SerializeField] private int maxFloor = 20;

        [SerializeField] private int currentRow = 10;
        [SerializeField] private int currentColumn = 1;
        [SerializeField] private int currentFloor = 1;

        [SerializeField] private BallPool ballPool;
        [SerializeField] private TrailManager trailManager;

        [SerializeField] private float waitForForwarding = 1.5f;

        private float currentWaitingTime;
        private Coroutine waitForwarding;

        private Dictionary<Trail, List<BallColumn>> ballColumns = new Dictionary<Trail, List<BallColumn>>();
        private void Start()
        {
            ballManager = this;
            StartCoroutine(InitiliazeBallManager());
        }

        public void StartForwading()
        {
            if (waitForwarding != null) currentWaitingTime += waitForForwarding;
            waitForwarding = StartCoroutine(Forwarding());
        }
        public IEnumerator InitiliazeBallManager()
        {
            ballPool = Instantiate(ballPool);
            ballPool.amountToPool = maxRow * maxColumn * maxFloor;
            yield return StartCoroutine(ballPool.StartInstantiatePool());
            trailManager = Instantiate(trailManager, transform);
            yield return StartCoroutine(trailManager.InstantiateTrailList(maxColumn, distance,playerRunner));
            List<Trail> trailList = trailManager.GetDeactivedList();
            CreateColumns(trailList);
            InstantiateStartBalls();
            yield return null;
        }
        
        private void InstantiateStartBalls()
        {
            for (int i = 0; i < currentColumn; i++)
            {
                Trail activetingTrail = trailManager.ActivetingTrail();
                for (int j = 0; j < currentRow; j++)
                {
                    BallColumn ballColumn = ballColumns[activetingTrail][j];
                    for (int k = 0; k < currentFloor; k++)
                    {
                        Ball ball = ballPool.GetPooledObject().GetComponent<Ball>();
                        ball.SetBall(ballColumn);
                    }
                }
            }
        }

        private void CreateColumns(List<Trail> trailList)
        {
            foreach (Trail trail in trailList)
            {
                for (int i = 0; i < maxColumn; i++)
                {
                    BallColumn CreatedBallColumn = Instantiate(ballColumn);
                    if (ballColumns.ContainsKey(trail))
                    {
                        CreatedBallColumn.InitializeBallColumn(ballColumns[trail].Last().transform, distance, maxFloor,
                            trail);
                    }
                    else
                    {
                        CreatedBallColumn.InitializeBallColumn(trail.transform, distance, maxFloor, trail);
                        ballColumns[trail] = new List<BallColumn>();
                    }

                    ballColumns[trail].Add(CreatedBallColumn);
                }
            }
        }
        public void RepositioningToForward()
        {

            foreach (KeyValuePair<Trail, List<BallColumn>> ballColumnList in ballColumns)
            {
                for (int i = 0; i < currentRow-1; i++)
                {
                    BallColumn ballColumn = ballColumnList.Value[i];
                    for (int j = i+1; j < currentRow; j++)
                    {
                        if (ballColumn.BallCount() >= currentFloor) break;
                        if (ballColumnList.Value[j].BallCount()  >ballColumn.BallCount())
                        {
                            ballColumnList.Value[j].GetBall(ballColumn.BallCount()).SetBall(ballColumn);
                            j--;
                        }
                    }
                }
            }
            //TODO maliyeti azalt
            
        }

        private IEnumerator Forwarding()
        {
            currentWaitingTime = waitForForwarding;
            while (currentWaitingTime >= 0)
            {
                currentWaitingTime -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            RepositioningToForward();
            waitForwarding = null;
            print("calisti");
            yield return null;
        }
    }
}