using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Gameplay.Runner.Gates;
using _Game.Scripts.Game.ObjectPools;
using _Game.Scripts.Game.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Game.Scripts.Game.Gameplay.Runner.Ball
{
    public class BallManager : MonoBehaviour
    {
        public static BallManager ballManager;
        public int totalBallCount;
        //TODO çok kötü
        [SerializeField] private Transform ballColumnTransform;
        [SerializeField] private Transform registeredTrailTransform;
        
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

        public void OnEnterGate(GateSpecs gateSpecs, Action DisableGate)
        {
            StartCoroutine(WaitUntilBallEnd(totalBallCount, gateSpecs, DisableGate));
        }

        public void StartForwading()
        {
            if (waitForwarding != null)
            {
                currentWaitingTime = waitForForwarding;
                return;
            }

            waitForwarding = StartCoroutine(Forwarding());
        }

        public IEnumerator InitiliazeBallManager()
        {
            ballPool = Instantiate(ballPool);
            ballPool.amountToPool = maxRow * maxColumn * maxFloor;
            yield return StartCoroutine(ballPool.StartInstantiatePool());
            trailManager = Instantiate(trailManager, transform);
            yield return StartCoroutine(trailManager.InstantiateTrailList(maxColumn, distance, playerRunner));
            List<Trail> trailList = trailManager.GetTrailList();
            CreateColumns(trailList);
            InstantiateStartBalls();
            yield return null;
        }

        private void InstantiateStartBalls()
        {
            for (int i = 0; i < currentColumn; i++)
            {
                Trail activetingTrail = trailManager.GetTrailList()[i];
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
                Transform parent = Instantiate(registeredTrailTransform);
                parent.parent = ballColumnTransform;
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

                    CreatedBallColumn.transform.parent = parent;
                    ballColumns[trail].Add(CreatedBallColumn);
                }
            }
        }

        public void RepositioningToForward()
        {
            List<Trail> activeTrailList = trailManager.GetActivatedTrailList();
            foreach (Trail trail in activeTrailList)
            {
                for (int i = 0; i < currentRow - 1; i++)
                {
                    BallColumn ballColumn = ballColumns[trail][i];
                    for (int j = i + 1; j < currentRow; j++)
                    {
                        if (ballColumn.BallCount() >= currentFloor) break;
                        if (ballColumns[trail][j].BallCount() > ballColumn.BallCount())
                        {
                            ballColumns[trail][j].GetBall(ballColumn.BallCount()).SetBall(ballColumn);
                            j--;
                        }
                    }
                }
            }
        }

        private IEnumerator Forwarding()
        {
            currentWaitingTime = waitForForwarding;
            while (currentWaitingTime >= 0)
            {
                currentWaitingTime -= 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            RepositioningToForward();
            waitForwarding = null;
            yield return null;
        }

        private IEnumerator WaitUntilBallEnd(int spawnBallCount, GateSpecs gateSpecs, Action DisableGate)
        {
            while (totalBallCount > 0)
            {
                yield return null;
            }
            DisableGate?.Invoke();
            currentColumn = gateSpecs.newColumn;
            currentFloor = gateSpecs.newFloor;
            for (int j = 0; j < currentRow; j++)
            {
                for (int i = 0; i < currentColumn; i++)
                {
                    for (int k = 0; k < currentFloor && spawnBallCount > 0; k++)
                    {
                        Ball ball = ballPool.GetPooledObject().GetComponent<Ball>();
                        ball.SetBall(ballColumns[trailManager.GetTrailList()[i]][j]);
                        spawnBallCount--;
                        
                    }
                }
                yield return null;
            }
        }
    }
}