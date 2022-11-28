using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.ColumnQueue;
using _Game.Scripts.Game.Gameplay.Runner.Gates;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using _Game.Scripts.Game.ObjectPools;
using UnityEngine;


namespace _Game.Scripts.Game.Gameplay.Runner.BallPositioning
{
    public class BallManager : MonoBehaviour
    {
        public static BallManager Instance;
        public Action CheckingCurrentRow;
        public Action CheckingCurrentFloor;
        private int totalBallCount;

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

        public void AddTotalBallCount(int count)
        {
            totalBallCount += count;
            if(totalBallCount<=0)
            {
                GameManager.Instance.OnGameOver();
            }
        }
        private void Awake()
        {
            Instance = this;
        }
        public IEnumerator  UpAdder(int size)
        {
            yield return StartCoroutine(GetCubicForm());
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
                yield return null;
            }
        }

        public IEnumerator RightAdder(int size)
        {
            yield return StartCoroutine(GetCubicForm());
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

        public List<Ball> GetFloors(int floorCount)
        {
            List<Ball> balls = new List<Ball>();
            Ball ball;
            List<ColumnHead> columnHeads=headsOrganizer.ColumnHeads;
            for (int i = 0; i < currentColumn; i++)
            {
                ColumnHead columnHead = columnHeads[i];
                for (int j = 0; j < currentRow; j++)
                {
                    BallColumn ballColumn = columnHead.BallColumns[j];
                    for (int k = 0; k < floorCount; k++)
                    {
                        ball = ballColumn.GetBallReference(k);
                        if(ball!=null)balls.Add(ball);
                    }
                }
            }

            return balls;
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
            yield return StartCoroutine(InstantiateBallPool());
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
                    { Ball ball=ballPool.GetPooledObject().GetComponent<Ball>();
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
        public void ReshapeWider(int newSize)
        {
            
            if (totalBallCount <= 0) return;
            List<Ball> repositionedBalls = new List<Ball>();
            repositionedBalls=ballPool.GetAllActiveBall();
            headsOrganizer.ClearAllColumns();
            int oneFloorMaxSize = newSize * maxRow;
            currentFloor = totalBallCount / oneFloorMaxSize;
            if (totalBallCount % oneFloorMaxSize > 0) currentFloor++;
            if (currentColumn <= 1)
            {
                currentRow = totalBallCount / newSize;
                if (totalBallCount % newSize > 0) currentRow++;
            }
            else currentRow = maxRow;
            currentColumn = newSize;
            RepositioningWiderBall(repositionedBalls);
        }
        public void ReshapeTaller(int newSize)
        {
            if (totalBallCount <= 0) return;
            List<Ball> repositionedBalls = new List<Ball>();
            repositionedBalls=ballPool.GetAllActiveBall();
            headsOrganizer.ClearAllColumns();
            int oneColumnMaxSize = newSize * maxRow;
            currentColumn = totalBallCount / oneColumnMaxSize;
            if (totalBallCount % oneColumnMaxSize > 0) currentColumn++; 
            currentRow = maxRow;
            currentFloor = newSize;
            RepositioningTallerBall(repositionedBalls);
        }

        private void RepositioningTallerBall(List<Ball> repositionedBalls)
        {
            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            if (currentColumn >= maxColumn) currentColumn = maxColumn;
            for (int i = 0; i < currentColumn; i++)
            {
                
                columnHead = headsOrganizer.ColumnHeads[i];
                for (int j = 0; j < currentRow; j++)
                {
                    ballColumn = columnHead.BallColumns[j];
                    for (int k = 0; k < currentFloor && repositionedBalls.Count > 0; k++)
                    {
                        ball = repositionedBalls[0];
                        ball.SwapColumn(ballColumn);
                        repositionedBalls.RemoveAt(0);
                    }
                }
            }

            foreach (Ball newBall in repositionedBalls)
            {
                newBall.RemoveBall();
                newBall.transform.parent = ballPool.transform;
            }
            headsOrganizer.StartCoroutine(headsOrganizer.SetPositionsInstantly());
        }
        private void RepositioningWiderBall(List<Ball> repositionedBalls)
        {
            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            if (currentColumn >= maxColumn) currentColumn = maxColumn;
            for (int i = 0; i < currentRow; i++)
            {
                for (int j = 0; j <currentColumn ; j++)
                {
                    columnHead = headsOrganizer.ColumnHeads[j];
                    ballColumn = columnHead.BallColumns[i];
                    for (int k = 0; k < currentFloor && repositionedBalls.Count > 0; k++)
                    {
                        ball = repositionedBalls[0];
                        ball.SwapColumn(ballColumn);
                        repositionedBalls.RemoveAt(0);
                    }
                }
            }

            foreach (Ball newBall in repositionedBalls)
            {
                newBall.RemoveBall();
                newBall.transform.parent = ballPool.transform;
            }
            headsOrganizer.StartCoroutine(headsOrganizer.SetPositionsInstantly());
        }


        private IEnumerator GetCubicForm()
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
            yield return null;
        }
    }
}