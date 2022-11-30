using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.ColumnQueue;
using _Game.Scripts.Game.Gameplay.Runner.Gates;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using _Game.Scripts.Game.ObjectPools;
using Unity.VisualScripting;
using UnityEngine;


namespace _Game.Scripts.Game.Gameplay.Runner.BallPositioning
{
    public class BallManager : MonoBehaviour
    {
        public static BallManager Instance;
        public Action CheckingCurrentRow;
        public Action CheckingCurrentFloor;
        public int TotalBallCount { get; private set; }

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

        public void AddTotalBallCount(int count)
        {
            TotalBallCount += count;
            if(TotalBallCount<=0)
            {
                GameManager.Instance.OnGameOver();
            }
        }
        
        public void RemoveBall(int ballCount)
        {
          
            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            for (int i = currentFloor-1; i >= 0; i--)
            {
                for (int j = currentColumn-1; j >=0 ; j--)
                {
                    columnHead = headsOrganizer.ColumnHeads[j];
                    for (int k = currentRow-1; k >= 0 ; k--)
                    {
                        ballColumn = columnHead.BallColumns[k];
                        if (ballColumn.BallCount() > i)
                        {
                            ball=ballColumn.GetBall(i);
                            ball.RemoveBallWithAnimation();
                            ballCount--;
                        }

                        if (ballCount == 0)
                        {
                            currentFloor = 0;
                            CheckingCurrentFloor?.Invoke();
                            return;
                                
                        }
                    }
                }
            }
            
        }
        public void AddBall(int ballCount)
        {
          
            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
                for (int i = 0; i < maxFloor; i++)
                {
                    for (int j = 0; j < currentColumn; j++)
                    {
                        columnHead = headsOrganizer.ColumnHeads[j];
                        for (int k = 0; k < currentRow; k++)
                        {
                            ballColumn = columnHead.BallColumns[k];
                            if (ballColumn.BallCount() < i)
                            {
                                ball = ballPool.GetPooledObject().GetComponent<Ball>();
                                ball.SetBall(ballColumn);
                                ballCount--;
                            }

                            if (ballCount == 0)
                            {
                                currentFloor = 0;
                                CheckingCurrentFloor?.Invoke();
                                return;
                                
                            }
                        }
                    }
                }
            
        }
        public IEnumerator UpAdder(int size)
        {
            yield return StartCoroutine(GetCubicForm());
            currentFloor += size;
            if (currentFloor > maxFloor)
            {
                currentFloor = maxFloor;
                yield break;
            }
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
        public IEnumerator UpRemover(int size)
        {
            currentFloor -= size;
            if (currentFloor < 0)
            {
                currentFloor = 0;
            }
            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            for (int i = currentFloor+size-1; i >= currentFloor; i--)
            {
                for (int j = currentColumn-1; j >=0 ; j--)
                {
                    columnHead = headsOrganizer.ColumnHeads[j];
                    for (int k = currentRow-1; k >= 0 ; k--)
                    {
                        ballColumn = columnHead.BallColumns[k];
                        if (ballColumn.BallCount() > i)
                        {
                            ball=ballColumn.GetBall(i);
                            ball.RemoveBallWithAnimation();
                        }
                    }
                }

                yield return null;
            }
            currentFloor = 0;
            CheckingCurrentFloor?.Invoke();
        }
        
        public IEnumerator RightRemover(int size)
        {
            currentColumn -= size;
            if (currentColumn < 0)
            {
                currentColumn = 0;
            }
            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            for (int i = currentFloor-1; i >= 0; i--)
            {
                for (int j = currentColumn+size-1; j >=currentColumn ; j--)
                {
                    columnHead = headsOrganizer.ColumnHeads[j];
                    for (int k = currentRow-1; k >= 0 ; k--)
                    {
                        ballColumn = columnHead.BallColumns[k];
                        if (ballColumn.BallCount() > i)
                        {
                            ball=ballColumn.GetBall(i);
                            ball.RemoveBallWithAnimation();
                        }
                    }
                }

                yield return null;
            }
            headsOrganizer.SetPositions();
            currentFloor = 0;
            CheckingCurrentFloor?.Invoke();
        }
        public IEnumerator LengthRemover(int size)
        {
            currentRow -= size;
            if (currentRow < 0)
            {
                currentRow = 0;
            }
            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            for (int i = currentFloor-1; i >= 0; i--)
            {
                for (int j = currentColumn-1; j >=0 ; j--)
                {
                    columnHead = headsOrganizer.ColumnHeads[j];
                    for (int k = currentRow+size-1; k >= currentRow ; k--)
                    {
                        ballColumn = columnHead.BallColumns[k];
                        if (ballColumn.BallCount() > i)
                        {
                            ball=ballColumn.GetBall(i);
                            ball.RemoveBallWithAnimation();
                        }
                    }
                }

                yield return null;
            }
            currentFloor = 0;
            CheckingCurrentRow?.Invoke();
        }

        public IEnumerator RightAdder(int size)
        {
            yield return StartCoroutine(GetCubicForm());
            currentColumn += size;
            if (currentColumn > maxColumn)
            {
                currentColumn = maxColumn;
                yield break;
            }
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
        
        public IEnumerator LengthAdder(int size)
        {
            yield return StartCoroutine(GetCubicForm());
            currentRow += size;
            if (currentRow > maxRow)
            {
                currentRow = maxRow;
                yield break;
            }
            for (int i = 0; i < currentColumn; i++)
            {
                for (int j = currentRow-size; j < currentRow; j++)
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
                        ball = ballColumn.GetBall(k);
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
            ColumnHead columnHead;
            BallColumn ballColumn;
            for (int i = 0; i < currentColumn; i++)
            {
                columnHead = columnHeads[i];
                for (int j = 0; j < currentRow; j++)
                {
                    ballColumn = columnHead.BallColumns[j];
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
            List<ColumnHead> columnHeads;
            BallColumn ballColumn;
            Ball ball;
            for (int i = 0; i < currentColumn; i++)
            {
               columnHeads=headsOrganizer.ColumnHeads;
                for (int j = 0; j < currentRow - 1; j++)
                {
                    ballColumn = columnHeads[i].BallColumns[j];
                    for (int k = j + 1; k < currentRow; k++)
                    {
                        if (ballColumn.BallCount() >= currentFloor) break;
                        if (columnHeads[i].BallColumns[k].BallCount() <= ballColumn.BallCount()) continue;

                        ball = columnHeads[i].BallColumns[k].GetBall(ballColumn.BallCount());
                        ball.SwapColumn(ballColumn);
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
            currentFloor = 0;
            currentRow = 0;
            CheckingCurrentRow?.Invoke();
            CheckingCurrentFloor?.Invoke();
            waitForwarding = null;
        }
        public void ReshapeWider(int newSize)
        {
            
            if (TotalBallCount <= 0) return;
            List<Ball> repositionedBalls = new List<Ball>();
            repositionedBalls=ballPool.GetAllActiveBall();
            headsOrganizer.ClearAllColumns();
            int oneFloorMaxSize = newSize * maxRow;
            currentFloor = TotalBallCount / oneFloorMaxSize;
            if (TotalBallCount % oneFloorMaxSize > 0) currentFloor++;
            if (currentColumn <= 1)
            {
                currentRow = TotalBallCount / newSize;
                if (TotalBallCount % newSize > 0) currentRow++;
            }
            else currentRow = maxRow;
            currentColumn = newSize;
            RepositioningWiderBall(repositionedBalls);
        }
        public void ReshapeTaller(int newSize)
        {
            if (TotalBallCount <= 0) return;
            List<Ball> repositionedBalls = new List<Ball>();
            repositionedBalls=ballPool.GetAllActiveBall();
            headsOrganizer.ClearAllColumns();
            int oneColumnMaxSize = newSize * maxRow;
            currentColumn = TotalBallCount / oneColumnMaxSize;
            if (TotalBallCount % oneColumnMaxSize > 0) currentColumn++; 
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