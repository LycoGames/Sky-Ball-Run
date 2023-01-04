using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.BallPositioning.ColumnQueue
{
    public class ColumnHead : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotateSpeed = 1f;
        [SerializeField] private float rotateStartAngle = 20f;
        [SerializeField] private BallColumn ballColumn;
        public List<BallColumn> BallColumns { get; private set; }

        private Vector3 follow;

        private void Start()
        {
        }

        public void ClearAllColumns()
        {
            foreach (BallColumn ballColumn in BallColumns)
            {
                ballColumn.ClearColumn();
            }
        }

        public IEnumerator InitializeColumnHead(int columnCount, float distance, int maxFloor)
        {
            BallManager.Instance.CheckingCurrentRow += CurrentRow;
            GameObject emptyGO = new GameObject();
            emptyGO.name = gameObject.name + " List";
            emptyGO.transform.parent = GameObject.Find("Column Heads").transform;
            yield return StartCoroutine(InstantiateBallColumns(emptyGO.transform, columnCount, maxFloor));
        }

        public IEnumerator InstantiateBallColumns(Transform parent, int columnCount, int maxFloor)
        {
            BallColumns = new List<BallColumn>();
            for (int i = 0; i < columnCount; i++)
            {
                BallColumns.Add(Instantiate(ballColumn, parent));
                if (i == 0)
                {
                    BallColumns.Last().InitializeBallColumn(transform, maxFloor);
                }
                else
                {
                    BallColumns.Last().InitializeBallColumn(BallColumns[i - 1].transform, maxFloor);
                }
            }

            yield return null;
        }

        public bool CheckIsActive()
        {
            //TODO Çok maliyetli
            int count = ActiveColumnCount();
            if (count > 0) return true;
            return false;
        }

        private int ActiveColumnCount()
        {
            int count = 0;
            foreach (BallColumn ballColumn in BallColumns)
            {
                if (ballColumn.CheckIsActive())
                {
                    count++;
                }
            }

            return count;
        }

        public void SetPosition(float posX)
        {
            StopAllCoroutines();
            StartCoroutine(RotateToDestination(posX));
            StartCoroutine(MoveToDestination(posX));
        }

        private void CurrentRow()
        {
            int count = ActiveColumnCount();
            if (count > BallManager.Instance.currentRow)
                BallManager.Instance.currentRow = count;
        }

        private IEnumerator RotateToDestination(float newX)
        {
            bool isPositive = (transform.localPosition.x - newX) >= 0;
            float yRotation = rotateStartAngle * (isPositive ? -1 : 1);
            transform.localRotation = Quaternion.Euler(0, yRotation, 0);
            while (transform.eulerAngles.y != 0)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0),
                    rotateSpeed * Time.deltaTime);
                yield return null;
            }

            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        private IEnumerator MoveToDestination(float newX)
        {
            Vector3 newPos = Vector3.zero;
            newPos.x = newX;
            while (Vector3.Distance(transform.localPosition, newPos) >= 0.03f)
            {
                transform.localPosition =
                    Vector3.MoveTowards(transform.localPosition, newPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.localPosition = newPos;
            yield return null;
        }
    }
}