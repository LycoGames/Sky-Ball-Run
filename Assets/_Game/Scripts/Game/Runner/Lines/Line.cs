using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Game.Runner.Lines
{
    public class Line : MonoBehaviour
    {
        public Action OnLinePassed;
        [SerializeField] private List<GameObject> obstacles;
        [SerializeField] private List<Transform> obstaclePositions;
        private List<Transform> activeObstacles =new List<Transform>();

        private void OnEnable()
        {
            CreateRandomObstacle();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnLinePassed?.Invoke();
                CreateRandomObstacle();
            }
        }

        private void CreateRandomObstacle()
        {
            if(activeObstacles.Any())DestroyAllObstacles();

            for (int i = 0; i < obstaclePositions.Count; i++)
            {
                int index = Random.Range(0, obstacles.Count);
                SpawnObstacle(obstacles[index], i);
            }
        }

        private void DestroyAllObstacles()
        {
            foreach (Transform obstacle in activeObstacles)
            {
                Destroy(obstacle.gameObject);
            }
            activeObstacles.Clear();
        }

        private void SpawnObstacle(GameObject obstacle, int index)
        {
            Vector3 pos = new Vector3(obstacle.transform.position.x, obstacle.transform.position.y, obstaclePositions[index].transform.position.z);
            activeObstacles.Add(Instantiate(obstacle, pos, obstacle.transform.rotation).transform);
        }
    }
}