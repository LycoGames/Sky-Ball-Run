using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.ColumnQueue;
using _Game.Scripts.Game.Gameplay.Runner.Gates;
using _Game.Scripts.Game.ObjectPools;
using UnityEngine;


namespace _Game.Scripts.Game.Gameplay.Runner.BallPositioning
{
    public class BallManager : MonoBehaviour
    {
        public static BallManager Instance;
        public Action CheckingCurrentRow;
        public Action CheckingCurrentFloor;
        public int totalBallCount;

        [SerializeField] private PlayerController playerController;
        [SerializeField] private float distance = 0.5f;
        public int maxRow = 30;
        public int maxColumn = 38;
        public int maxFloor = 20;

        public int currentRow = 10;
        public int currentColumn = 1;
        public int currentFloor = 1;

        [SerializeField] private BallPool ballPool;
        [SerializeField] private HeadsOrganizer headsOrganizer;

        [SerializeField] private float waitForForwarding = 1.5f;

        private float currentWaitingTime;
        private Coroutine waitForwarding;
        

        private void Awake()
        {
            Instance = this;
        }
        
        public void OnEnterGate(GateSpecs gateSpecs, Action disableGate)
        {
            StartCoroutine(WaitUntilBallEnd(totalBallCount, gateSpecs, disableGate));
        }

        public void StartForwarding()
        {
            if (waitForwarding != null)
            {
                currentWaitingTime = waitForForwarding;
                return;
            }

            waitForwarding = StartCoroutine(Forwarding());
        }
    
        public IEnumerator InitializeBallManager()
        {
            yield return InstantiateBallPool();
            yield return StartCoroutine(headsOrganizer.InitializeHeadsOrganizer(maxColumn, distance, playerController, maxFloor, maxRow));
            yield return StartCoroutine(InstantiateStartBalls());
            yield return StartCoroutine(headsOrganizer.SetPositionsInstantly());
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
        

        private void RepositioningToForward()
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
                        if (columnHeads[i].BallColumns[k].BallCount() <= ballColumn.BallCount()) continue;
                        columnHeads[i].BallColumns[k].GetBall(ballColumn.BallCount()).SwapColumn(ballColumn);
                        k--;
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
            for (int j = 0; j < maxRow; j++)
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
            
            StartCoroutine(headsOrganizer.SetPositionsInstantly());
        }

        public void UpAdder(int size)
        {
            GetCubicForm();
            currentFloor += size;
            for (int i = 0; i < currentColumn; i++)
            {
                for (int j = 0; j < currentRow; j++)
                {
                    for (int k =  currentFloor - size; k < currentFloor; k++)
                    {
                        BallColumn ballColumn = headsOrganizer.ColumnHeads[i].BallColumns[j];
                        Ball ball = ballPool.GetPooledObject().GetComponent<Ball>();
                        ball.SetBall(ballColumn);
                    }
                }
            }
        }

        public void RightAdder(int size)
        {
            GetCubicForm();
            currentColumn += size;
            for (int i = currentColumn-size; i < currentColumn; i++)
            {
                for (int j = 0; j < currentRow; j++)
                {
                    for (int k = 0; k < currentFloor; k++)
                    {
                        BallColumn ballColumn = headsOrganizer.ColumnHeads[i].BallColumns[j];
                        Ball ball = ballPool.GetPooledObject().GetComponent<Ball>();
                        ball.SetBall(ballColumn);
                    }
                }
            }
            headsOrganizer.SetPositions();
        }

        private void GetCubicForm()
        {
            currentFloor = 0;
            currentRow = 0;
            CheckingCurrentRow?.Invoke();
            CheckingCurrentFloor?.Invoke();
            for (int i = 0; i < currentColumn; i++)
            {
                for (int j = 0; j < currentRow; j++)
                {
                    for (int k = 0; k < currentFloor; k++)
                    {
                        BallColumn ballColumn = headsOrganizer.ColumnHeads[i].BallColumns[j];
                        if (ballColumn.BallCount()<currentFloor)
                        {
                            Ball ball = ballPool.GetPooledObject().GetComponent<Ball>();
                            ball.SetBall(ballColumn);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            
        }
    }
}