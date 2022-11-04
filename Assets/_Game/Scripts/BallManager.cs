using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts
{
    public class BallManager : MonoBehaviour
    {
        public static BallManager ballManager;
        [SerializeField] public Ball ball;
        [SerializeField] private float floorDistance;
        [SerializeField] private int maxRow;
        [SerializeField] private int maxColumn;
        [SerializeField] private int maxFloor;
        [SerializeField] private Floor floor;
        [SerializeField] private int firstSpawnBallCount = 50;
        private Dictionary<bool, List<Floor>> floorsList = new Dictionary<bool, List<Floor>>();
        private int currentFloor;
        private int totalBallCount;

        public int GetMaxRow() => maxRow;
        public int GetMaxColumn() => maxColumn;

        void Awake()
        {
            ballManager = this;
            InitiliazeFloorList();
            InitiliazeFloors(maxFloor);
            InitiliazeFirstFloor();
        }

        void Start()
        {
            SpawnBallByCount(firstSpawnBallCount);
        }

        public void AddColumn()
        {
            maxFloor++;
            InitiliazeFloors(1);
            SpawnBallByCount(maxColumn*maxRow);

        }
        public void SpawnBallByCount(int ballCount)
        {
            for (int i = 0; i < ballCount; i++) AddBall();
        }

        public void EnableFloor(Floor floor)
        {
            floorsList[true].Add(floor);
            floorsList[false].Remove(floor);
            RePositioning();
        }

        public void DisableFloor(Floor floor)
        {
            floorsList[true].Remove(floor);
            floorsList[false].Add(floor);
            RePositioning();
        }

        public void RemoveBall()
        {
            totalBallCount--;
        }

        public void AddBall()
        {
            if (totalBallCount >= maxColumn * maxFloor * maxRow) return;
            if (floorsList[true].Count <= currentFloor)
            {
                if (!floorsList[false].Any()) return;
                EnableFloor(floorsList[false][0]);
            }
            if(!floorsList[true][currentFloor].AddBall())
            {
                currentFloor++;
                if (currentFloor >= maxFloor) currentFloor = 0;
                AddBall();
                return;
            }
            totalBallCount++;
        }


        private void Initiliaze()
        {
            InitiliazeFloorList();
            InitiliazeFirstFloor();
        }

        private void InitiliazeFirstFloor()
        {
            EnableFloor(floorsList[false][0]);
        }

        private void InitiliazeFloorList()
        {
            floorsList.Add(false, new List<Floor>());
            floorsList.Add(true, new List<Floor>());
        }

        private void InitiliazeFloors(int floorCount)
        {
            for (int i = 0; i < floorCount; i++)
            {
                floorsList[false].Add(Instantiate(floor, transform));
                floorsList[false][i].Initiliaze();
            }
        }

        private void RePositioning()
        {
            float yPos = 0;
            foreach (Floor floor in floorsList[true])
            {
                floor.transform.localPosition = new Vector3(0, yPos, 0);
                yPos += floorDistance;
            }
        }
    }
}