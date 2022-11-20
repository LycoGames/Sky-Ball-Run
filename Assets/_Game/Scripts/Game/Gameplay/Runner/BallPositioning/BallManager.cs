using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.ColumnQueue;
using _Game.Scripts.Game.Gameplay.Runner.Gates;
using _Game.Scripts.Game.ObjectPools;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Gameplay.Runner.BallPositioning
{
    public class BallManager : MonoBehaviour
    {
        public static BallManager ballManager;
        public int totalBallCount;

        [FormerlySerializedAs("playerRunner")] [SerializeField] private PlayerController playerController;
        [SerializeField] private float distance = 0.5f;
        [SerializeField] private int maxRow = 30;
        [SerializeField] private int maxColumn = 38;
        [SerializeField] private int maxFloor = 20;

        [SerializeField] private int currentRow = 10;
        [SerializeField] private int currentColumn = 1;
        [SerializeField] private int currentFloor = 1;

        [SerializeField] private BallPool ballPool;
        [SerializeField] private HeadsOrganizer headsOrganizer;

        [SerializeField] private float waitForForwarding = 1.5f;

        private float currentWaitingTime;
        private Coroutine waitForwarding;
        

        private void Awake()
        {
            ballManager = this;
            //StartCoroutine(InitiliazeBallManager());
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
            yield return InstantiateBallPool();
            yield return StartCoroutine(headsOrganizer.InitializeHeadsOrganizer(maxColumn, distance, playerController, maxFloor, maxRow));
            yield return StartCoroutine(InstantiateStartBalls());
            yield return StartCoroutine(headsOrganizer.SetPositionsInstantly(currentColumn));
            yield return null;
        }

        private IEnumerator InstantiateBallPool()
        {
            ballPool = Instantiate(ballPool);
            ballPool.amountToPool = maxRow * maxColumn * maxFloor;
            yield return StartCoroutine(ballPool.StartInstantiatePool());
            
        }

        private IEnumerator InstantiateStartBalls()
        {
            List<ColumnHead> columnHeads = headsOrganizer.ColumnHeads;
            for (int i = 0; i < currentColumn; i++)
            {
                ColumnHead columnHead = columnHeads[i];
                for (int j = 0; j < currentRow; j++)
                {
                    BallColumn ballColumn = columnHead.BallColumns[j];
                    for (int k = 0; k < currentFloor; k++)
                    {
                        Ball ball=ballPool.GetPooledObject().GetComponent<Ball>();
                        ball.SetBall(ballColumn);
                        
                    }
                }
            }
            yield return null;
        }
        

        public void RepositioningToForward()
        {
            
            for (int i = 0; i < currentColumn; i++)
            {
                List<ColumnHead> columnHeads=headsOrganizer.ColumnHeads;
                for (int j = 0; j < currentRow - 1; j++)
                {
                    BallColumn ballColumn = columnHeads[i].BallColumns[j];
                    for (int k = j + 1; k < currentRow; k++)
                    {
                        if (ballColumn.BallCount() >= currentFloor) break;
                        if (columnHeads[i].BallColumns[k].BallCount() > ballColumn.BallCount())
                        {
                            columnHeads[i].BallColumns[k].GetBall(ballColumn.BallCount()).SwapColumn(ballColumn);
                            k--;
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
            headsOrganizer.SetPositions();
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
                    BallColumn ballColumn = headsOrganizer.ColumnHeads[i].BallColumns[j];
                    for (int k = 0; k < currentFloor && spawnBallCount > 0; k++)
                    {
                        Ball ball = ballPool.GetPooledObject().GetComponent<Ball>();
                        ball.SetBall(ballColumn);
                        spawnBallCount--;
                        
                    }
                }
            }
            StartCoroutine(headsOrganizer.SetPositionsInstantly(currentColumn));
        }
    }
}