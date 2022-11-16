using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Player;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Ball
{
    public class TrailManager : MonoBehaviour
    { 
        [SerializeField] private Trail trail;
        private PlayerRunner playerRunner;
        private List<Trail> trailList = new List<Trail>();
        private float distance;


        public IEnumerator InstantiateTrailList(int trailCount,float _distance,PlayerRunner _playerRunner)
        {
            playerRunner = _playerRunner;
            distance = _distance;
            for (int i = 0; i < trailCount; i++)
            {
                trailList.Add(Instantiate(trail, transform));
                trailList.Last().InitiliazeTrail(this);
            }

            yield return null;
        }

        public List<Trail> GetTrailList()
        {
            return trailList;
        }
        public List<Trail> GetActivatedTrailList()
        {
            List<Trail> activeTrailList=new List<Trail>();
            foreach (Trail trail in trailList)
            {
                if(trail.isActive) activeTrailList.Add(trail);
            }

            return activeTrailList;
        }

        public void SetPositions()
        {
            List<Trail> activeTrailList = GetActivatedTrailList();
            int count=activeTrailList.Count;
            int div = count / 2;
            float currentDistance = ((distance * div) - distance / 2 * (1-(count % 2)))*-1;
            foreach (Trail trail in activeTrailList)
            {
                trail.SetPosition(currentDistance);
                currentDistance += distance;
            }
            playerRunner.ChangeBounds(currentDistance+distance);
        }
    }
}