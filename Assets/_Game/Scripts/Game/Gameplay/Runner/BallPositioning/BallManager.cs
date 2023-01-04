using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.ColumnQueue;
using _Game.Scripts.Game.Gameplay.Runner.LevelSystems;
using _Game.Scripts.Game.Gameplay.Runner.Lines;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using _Game.Scripts.Game.ObjectPools;
using Cinemachine;
using UnityEngine;


namespace _Game.Scripts.Game.Gameplay.Runner.BallPositioning
{
    public class BallManager : MonoBehaviour
    {
        public static BallManager Instance;
        public Action CheckingCurrentColumn;
        public Action CheckingCurrentRow;
        public Action CheckingCurrentFloor;
        public Action<float> ChangeCameraYPos;
        public Action<int> OnTotalBallCountChange;
        public Action OnGateCountCheck;
        public Action OnShapeChange;
        public int TotalBallCount { get; private set; }

        [SerializeField] private PlayerController playerController;

        [SerializeField] private float distance = 0.5f;
        public int MaxBallCount { get; private set; }

        public int maxRow = 30;
        public int maxColumn = 38;
        public int maxFloor = 20;

        public int currentRow = 10;
        public int currentColumn = 1;
        public int currentFloor = 1;

        [SerializeField] private int reviveRow = 3;
        [SerializeField] private int reviveColumn = 2;
        [SerializeField] private int reviveFloor = 2;

        private BallPool ballPool;
        [SerializeField] private HeadsOrganizer headsOrganizer;
        [SerializeField] private float waitForForwarding = 1.5f;

        private float currentWaitingTime;
        private Coroutine waitForwarding;

        private void Awake()
        {
            Instance = this;
        }

        public void DestroyBallManager()
        {
            headsOrganizer.destroyColumnHeads();
        }

        public void ClearAllColumns() => headsOrganizer.ClearAllColumns();

        public IEnumerator InitializeBallManager(BallPool _ballPool, PlayerController _playerController,
            BallSpecs ballSpecs, CinemachineVirtualCamera playerFollowerCamera)
        {
            GameManager.Instance.OnRevive += SpawnRevievBalls;
            ballPool = _ballPool;
            MaxBallCount = currentColumn * currentRow * currentFloor * 4;
            ballPool.amountToPool = MaxBallCount;
            playerController = _playerController;
            yield return StartCoroutine(ballPool.StartInstantiatePool());
            yield return StartCoroutine(
                headsOrganizer.InitializeHeadsOrganizer(maxColumn, distance, playerController, maxFloor, maxRow));
            yield return StartCoroutine(InstantiateStartBalls(ballSpecs.column, ballSpecs.floor, ballSpecs.row));
            yield return StartCoroutine(headsOrganizer.SetPositionsInstantly());
        }

        public void AddTotalBallCount(int count)
        {
            TotalBallCount += count;
            OnTotalBallCountChange?.Invoke(TotalBallCount);
            OnGateCountCheck?.Invoke();
            if (TotalBallCount <= 0)
            {
                GameManager.Instance.OnLoseGame();
            }
        }

        public IEnumerator RemoveBall(int ballCount)

        {
            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            for (int i = currentFloor - 1; i >= 0; i--)
            {
                for (int j = currentColumn - 1; j >= 0; j--)
                {
                    columnHead = headsOrganizer.ColumnHeads[j];
                    for (int k = currentRow - 1; k >= 0; k--)
                    {
                        ballColumn = columnHead.BallColumns[k];
                        if (ballColumn.BallCount() > i)
                        {
                            ball = ballColumn.GetBall(i);
                            ball.RemoveBallWithAnimation();
                            ballCount--;
                        }

                        if (ballCount == 0)
                        {
                            currentFloor = 0;
                            CheckFloorSizeAndMoveCam();
                            yield break;
                        }
                    }

                    yield return null;
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
                            CheckFloorSizeAndMoveCam();
                            PlayBallAddSound();
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
                    for (int k = currentFloor - size; k < currentFloor; k++)
                    {
                        BallColumn ballColumn = headsOrganizer.ColumnHeads[i].BallColumns[j];
                        Ball ball = ballPool.GetPooledObject().GetComponent<Ball>();
                        ball.SetBall(ballColumn);
                    }
                }
            }

            PlayBallAddSound();
            CheckCurrentSizes();
            OnGateCountCheck?.Invoke();
            OnShapeChange?.Invoke();
            SetCameraPos();
        }

        public IEnumerator UpRemover(int size)
        {
            currentFloor = 0;
            CheckingCurrentFloor?.Invoke();
            currentFloor -= size;
            if (currentFloor < 0)
            {
                currentFloor = 0;
            }

            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            for (int i = currentFloor + size - 1; i >= currentFloor; i--)
            {
                for (int j = currentColumn - 1; j >= 0; j--)
                {
                    columnHead = headsOrganizer.ColumnHeads[j];
                    for (int k = currentRow - 1; k >= 0; k--)
                    {
                        ballColumn = columnHead.BallColumns[k];
                        if (ballColumn.BallCount() > i)
                        {
                            ball = ballColumn.GetBall(i);
                            ball.RemoveBallWithAnimation();
                        }
                    }
                }

                yield return null;
            }

            CheckCurrentSizes();
            OnGateCountCheck?.Invoke();
            OnShapeChange?.Invoke();
            CheckFloorSizeAndMoveCam();
        }

        public int GetBallCountOnRemovedFloor(int value)
        {
            if (value >= currentFloor) return TotalBallCount;
            return headsOrganizer.GetBallCountOnRemovedFloor(currentFloor - value);
        }

        public IEnumerator RightRemover(int size)
        {
            List<ColumnHead> columnHeads = headsOrganizer.GetActiveList();
            currentColumn = columnHeads.Count;
            currentColumn -= size;
            if (currentColumn < 0)
            {
                currentColumn = 0;
            }

            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            int startIndex = (currentColumn - size) / 2;
            for (int i = currentFloor - 1; i >= 0; i--)
            {
                for (int j = startIndex; j < startIndex + size; j++)
                {
                    columnHead = columnHeads[j];
                    for (int k = currentRow - 1; k >= 0; k--)
                    {
                        ballColumn = columnHead.BallColumns[k];
                        if (ballColumn.BallCount() > i)
                        {
                            ball = ballColumn.GetBall(i);
                            ball.RemoveBallWithAnimation();
                        }
                    }
                }

                yield return null;
            }

            headsOrganizer.SetPositions();
            CheckCurrentSizes();
            OnGateCountCheck?.Invoke();
            OnShapeChange?.Invoke();
            CheckFloorSizeAndMoveCam();
        }

        public int GetBallCountOnRemovedColumn(int value)
        {
            if (value >= currentColumn) return TotalBallCount;
            return headsOrganizer.GetBallCountOnRemovedColumn(value);
        }

        public IEnumerator LengthRemover(int size)
        {
            currentRow = 0;
            CheckingCurrentRow?.Invoke();
            currentRow -= size;
            if (currentRow < 0)
            {
                currentRow = 0;
            }

            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            for (int i = currentFloor - 1; i >= 0; i--)
            {
                for (int j = currentColumn - 1; j >= 0; j--)
                {
                    columnHead = headsOrganizer.ColumnHeads[j];
                    for (int k = currentRow + size - 1; k >= currentRow; k--)
                    {
                        ballColumn = columnHead.BallColumns[k];
                        if (ballColumn.BallCount() > i)
                        {
                            ball = ballColumn.GetBall(i);
                            ball.RemoveBallWithAnimation();
                        }
                    }
                }

                yield return null;
            }

            CheckCurrentSizes();
            OnGateCountCheck?.Invoke();
            OnShapeChange?.Invoke();
        }

        public int GetBallCountOnRemovedRow(int value)
        {
            if (value >= currentRow) return TotalBallCount;
            return headsOrganizer.GetBallCountOnRemovedRow(currentRow - value);
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

            for (int i = currentColumn - size; i < currentColumn; i++)
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

            PlayBallAddSound();
            headsOrganizer.SetPositions();
            CheckCurrentSizes();
            OnGateCountCheck?.Invoke();
            OnShapeChange?.Invoke();
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
                for (int j = currentRow - size; j < currentRow; j++)
                {
                    for (int k = 0; k < currentFloor; k++)
                    {
                        BallColumn ballColumn = headsOrganizer.ColumnHeads[i].BallColumns[j];
                        Ball ball = ballPool.GetPooledObject().GetComponent<Ball>();
                        ball.SetBall(ballColumn);
                    }
                }
            }

            PlayBallAddSound();
            headsOrganizer.SetPositions();
            CheckCurrentSizes();
            OnGateCountCheck?.Invoke();
            OnShapeChange?.Invoke();
        }

        public List<Ball> GetBalls(int ballCount)
        {
            List<Ball> balls = new List<Ball>();
            Ball ball;
            List<ColumnHead> columnHeads = headsOrganizer.GetActiveList();
            currentColumn = columnHeads.Count;
            BallColumn ballColumn;
            for (int i = 0; i < currentFloor; i++)

            {
                for (int j = 0; j < currentRow; j++)
                {
                    for (int k = 0; k < currentColumn && ballCount > 0 && TotalBallCount > 0; k++)
                    {
                        ballColumn = columnHeads[k].BallColumns[j];
                        ball = ballColumn.GetBall(i);
                        if (ball == null) continue;
                        ballCount--;
                        balls.Add(ball);
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

        private void SpawnRevievBalls()
        {
            StartCoroutine(InstantiateStartBalls(reviveColumn, reviveFloor, reviveRow));
        }

        private IEnumerator InstantiateStartBalls(int column, int floor, int row)
        {
            currentColumn = column;
            currentFloor = floor;
            currentRow = row;
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
                    {
                        Ball ball = ballPool.GetPooledObject().GetComponent<Ball>();
                        ball.SetBall(ballColumn);
                    }
                }
            }

            SetCameraPos();
            headsOrganizer.StartCoroutine(headsOrganizer.SetPositionsInstantly());
            OnGateCountCheck?.Invoke();
            yield return null;
        }


        private void RepositioningToForward()
        {
            List<ColumnHead> columnHeads;
            BallColumn ballColumn;
            Ball ball;
            for (int i = 0; i < currentColumn; i++)
            {
                columnHeads = headsOrganizer.ColumnHeads;
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

        public void ReshapeWider(int newSize)
        {
            if (TotalBallCount <= 0) return;
            if (TotalBallCount <= 1)
            {
                PlayReshapeSound();
                return;
            }

            List<Ball> repositionedBalls = new List<Ball>();
            repositionedBalls = ballPool.GetAllActiveBall();
            headsOrganizer.ClearAllColumns();

            currentColumn = newSize;

            int rowFloorCount = TotalBallCount / currentColumn;
            currentRow = maxRow;
            currentFloor = rowFloorCount / currentRow;
            if (rowFloorCount % currentRow > 0) currentFloor++;
            if (currentFloor >= maxFloor) currentFloor = maxFloor;
            if (currentFloor <= 0) currentFloor = 1;


            PlayReshapeSound();
            SetCameraPos();
            RepositioningWiderBall(repositionedBalls);
            CheckCurrentSizes();
            OnGateCountCheck?.Invoke();
            OnShapeChange?.Invoke();
        }


        public void ReshapeTaller(int newSize)
        {
            if (TotalBallCount <= 0) return;
            if (TotalBallCount <= 1)
            {
                PlayReshapeSound();
                return;
            }

            List<Ball> repositionedBalls = new List<Ball>();
            repositionedBalls = ballPool.GetAllActiveBall();
            headsOrganizer.ClearAllColumns();

            currentFloor = newSize;

            int rowCount = TotalBallCount / currentFloor;
            if (TotalBallCount % currentFloor > 0) rowCount++;

            currentColumn = rowCount / maxRow;
            if (rowCount % maxRow > 0) currentColumn++;

            currentRow = rowCount / currentColumn;
            if (rowCount % currentColumn > 0) currentRow++;

            PlayReshapeSound();
            SetCameraPos();
            RepositioningTallerBall(repositionedBalls);
            CheckCurrentSizes();
            OnGateCountCheck?.Invoke();
            OnShapeChange?.Invoke();
        }

        private void CheckCurrentSizes()
        {
            currentFloor = 0;
            currentRow = 0;
            currentColumn = 0;

            CheckingCurrentFloor?.Invoke();
            CheckingCurrentRow?.Invoke();
            CheckingCurrentColumn?.Invoke();
        }

        public void ReshapeBalls(int newRow, int newColumn)
        {
            if (TotalBallCount <= 1) return;
            int newfloor = TotalBallCount / (newRow * newColumn);
            newfloor += TotalBallCount % (newRow * newColumn) > 0 ? 1 : 0;
            while (newfloor > maxFloor)
            {
                if (newColumn <= maxColumn) newColumn++;
                else if (newRow <= maxRow) newRow++;
                newfloor = TotalBallCount / newRow * newColumn;
                if (newColumn >= maxColumn && newRow >= maxRow) break;
            }

            Debug.Log("row: " + newRow + " column: " + newColumn + " floor: " + newfloor);
            Debug.Break();
            currentFloor = newfloor;
            currentColumn = newColumn;
            currentRow = newRow;
            PlayReshapeSound();
            SetCameraPos();
            RepositioningBalls();
            CheckCurrentSizes();
            OnGateCountCheck?.Invoke();
            OnShapeChange?.Invoke();
        }

        private void RepositioningBalls()
        {
            List<Ball> repositionedBalls = ballPool.GetAllActiveBall();
            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            if (currentColumn >= maxColumn) currentColumn = maxColumn;
            for (int i = 0; i < currentColumn; i++)
            {
                columnHead = headsOrganizer.ColumnHeads[i];
                for (int j = 0; j < currentFloor; j++)
                {
                    for (int k = 0; k < currentRow && repositionedBalls.Count > 0; k++)
                    {
                        ballColumn = columnHead.BallColumns[k];
                        ball = repositionedBalls[0];
                        ball.SwapColumn(ballColumn);
                        repositionedBalls.RemoveAt(0);
                    }
                }
            }

            if (repositionedBalls.Any())
            {
                Debug.LogWarning("Error: Remind ball on reshaping");
                int removedBallCount = repositionedBalls.Count;
                foreach (Ball newBall in repositionedBalls)
                {
                    newBall.RemoveBall();
                    newBall.transform.parent = ballPool.transform;
                }

                AddBall(removedBallCount);
            }
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
                for (int j = 0; j < currentFloor; j++)
                {
                    for (int k = 0; k < currentRow && repositionedBalls.Count > 0; k++)
                    {
                        ballColumn = columnHead.BallColumns[k];
                        ball = repositionedBalls[0];
                        ball.SwapColumn(ballColumn);
                        repositionedBalls.RemoveAt(0);
                    }
                }
            }

            headsOrganizer.StartCoroutine(headsOrganizer.SetPositionsInstantly());
            //TODO top fazladan siliniyor.
            if (repositionedBalls.Any())
            {
                int removedBallCount = repositionedBalls.Count;
                foreach (Ball newBall in repositionedBalls)
                {
                    newBall.RemoveBall();
                    newBall.transform.parent = ballPool.transform;
                }

                AddBall(removedBallCount);
            }
        }

        private void RepositioningWiderBall(List<Ball> repositionedBalls)
        {
            BallColumn ballColumn;
            ColumnHead columnHead;
            Ball ball;
            if (currentColumn >= maxColumn) currentColumn = maxColumn;
            for (int i = 0; i < currentRow; i++)
            {
                for (int j = 0; j < currentColumn; j++)
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

            headsOrganizer.StartCoroutine(headsOrganizer.SetPositionsInstantly());

            if (repositionedBalls.Any())
            {
                int removedBallCount = repositionedBalls.Count;
                foreach (Ball newBall in repositionedBalls)
                {
                    newBall.RemoveBall();
                    newBall.transform.parent = ballPool.transform;
                }

                AddBall(removedBallCount);
            }
        }


        private IEnumerator GetCubicForm()
        {
            currentFloor = 0;
            currentRow = 0;
            CheckingCurrentRow?.Invoke();
            CheckFloorSizeAndMoveCam();
            for (int i = 0; i < currentColumn; i++)
            {
                for (int j = 0; j < currentRow; j++)
                {
                    for (int k = 0; k < currentFloor; k++)
                    {
                        BallColumn ballColumn = headsOrganizer.ColumnHeads[i].BallColumns[j];
                        if (ballColumn.BallCount() < currentFloor)
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
            CheckFloorSizeAndMoveCam();
            waitForwarding = null;
        }

        private void CheckFloorSizeAndMoveCam()
        {
            CheckingCurrentFloor?.Invoke();
            SetCameraPos();
        }

        private void SetCameraPos()
        {
            ChangeCameraYPos?.Invoke(currentFloor);
            // float endValue = stockCameraTrackingY + (float)currentFloor * 2 / 3;
            // DOTween.To(() => playerFollowerTarget.m_TrackedObjectOffset.y,
            //     x => playerFollowerTarget.m_TrackedObjectOffset.y = x, endValue, .5f);
            //
            // endValue = stockCameraDistance + (float)currentFloor / 3;
            // DOTween.To(() => playerFollowerTarget.m_CameraDistance,
            //     x => playerFollowerTarget.m_CameraDistance = x, endValue, .5f);
            // playerFollowerTarget.m_TrackedObjectOffset.y = stockCameraTrackingY + currentFloor * distance;
            // playerFollowerTarget.m_CameraDistance = stockCameraDistance + currentFloor * distance;
        }

        private static void PlayBallAddSound()
        {
            AudioSourceController.Instance.PlaySoundType(SoundType.BallAdd);
        }

        private void PlayReshapeSound()
        {
            AudioSourceController.Instance.PlaySoundType(SoundType.ShapeChange);
        }
    }
}