using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void InitializeColumnHead(int columnCount, float distance, int maxFloor)
        {
            GameObject emptyGO = new GameObject();
            emptyGO.name = gameObject.name + " List";
            InstantiateBallColumns(emptyGO.transform, columnCount, distance, maxFloor);
        }

        public void InstantiateBallColumns(Transform parent, int columnCount, float distance, int maxFloor)
        {
            BallColumns = new List<BallColumn>();
            for (int i = 0; i < columnCount; i++)
            {
                BallColumns.Add(Instantiate(ballColumn, parent));
                if (i == 0)
                {
                    BallColumns.Last().InitializeBallColumn(transform, distance, maxFloor);
                }
                else
                {
                    BallColumns.Last().InitializeBallColumn(BallColumns[i - 1].transform, distance, maxFloor);
                }
            }
        }

        public bool CheckIsActive()
        {
            //TODO Çok maliyetli
            int count=0;
            foreach (BallColumn ballColumn in BallColumns)
            {
                if (ballColumn.CheckIsActive())
                {
                    count++;
                }
            }
            if (count > 0) return true;
            return false;
        }

        public void SetPosition(float posX)
        {
            print("calist");
            StopAllCoroutines();
            StartCoroutine(RotateToDestination(posX));
            StartCoroutine(MoveToDestination(posX));
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
            yield return null;
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