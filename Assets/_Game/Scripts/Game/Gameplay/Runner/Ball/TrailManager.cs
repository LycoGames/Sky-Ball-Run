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

        private List<Trail> activeTrailList = new List<Trail>();
        private List<Trail> deactiveTrailList = new List<Trail>();
        private float distance;


        public IEnumerator InstantiateTrailList(int trailCount,float _distance,PlayerRunner _playerRunner)
        {
            playerRunner = _playerRunner;
            distance = _distance;
            for (int i = 0; i < trailCount; i++)
            {
                deactiveTrailList.Add(Instantiate(trail, transform));
                deactiveTrailList.Last().InitiliazeTrail(this);
            }

            yield return null;
        }

        public List<Trail> GetDeactivedList()
        {
            return deactiveTrailList;
        }

        public void DeactivetingTrail(Trail trail)
        {
            activeTrailList.Remove(trail);
            deactiveTrailList.Add(trail);
            SetPositions();
        }

        public Trail ActivetingTrail()
        {
            if (!deactiveTrailList.Any()) return null;
            Trail trail = deactiveTrailList.First();
            deactiveTrailList.Remove(trail);
            activeTrailList.Add(trail);
            SetPositions();
            return trail;
        }

        public void SetPositions()
        {
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