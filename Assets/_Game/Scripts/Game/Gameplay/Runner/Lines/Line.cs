using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Game.Gameplay.Runner.Lines
{
    public class Line : MonoBehaviour
    {
        [SerializeField] private List<Transform> obstaclePositions;
        
        private Action<int> onLinePassed;
        private List<Transform> activeObstacles =new List<Transform>();
        private int index;

        public void InitializeLine(List<GameObject>interactables,Action<int> _onLinePassed,int _index)
        {
            index = _index;
            onLinePassed += _onLinePassed;
            CreateInteractables(interactables);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onLinePassed?.Invoke(index);
                gameObject.SetActive(false);
            }
        }

        private void CreateInteractables(List<GameObject>interactables)
        {
            for (int i = 0; i < obstaclePositions.Count; i++)
            {
                if (i>=interactables.Count) break;
                SpawnInteractables(interactables[i], i);
            }
        }
        

        private void SpawnInteractables(GameObject obstacle, int index)
        {
            Vector3 pos = new Vector3(obstacle.transform.position.x, obstacle.transform.position.y, obstaclePositions[index].transform.position.z);
            GameObject created = Instantiate(obstacle, pos, obstacle.transform.rotation,transform);
            activeObstacles.Add(created.transform);
        }
    }
}