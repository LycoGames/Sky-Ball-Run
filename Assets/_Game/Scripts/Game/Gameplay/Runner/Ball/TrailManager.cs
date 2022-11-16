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
        private List<Trail> trailList = new List<Trail>();
        private float distance;


        public IEnumerator InstantiateTrailList(int trailCount,float _distance,PlayerRunner _playerRunner)
        {
            playerRunner = _playerRunner;
            distance = _distance;
            for (int i = 0; i < trailCount; i++)
            {
                deactiveTrailList.Add(Instantiate(trail, transform));
                trailList.Add(deactiveTrailList.Last());
                deactiveTrailList.Last().InitiliazeTrail(this);
            }

            yield return null;
        }

        public void ResetList()
        {
            activeTrailList.Clear();
            deactiveTrailList.Clear();
            foreach (Trail trail in trailList)
            {
                deactiveTrailList.Add(trail);
                trail.SetZero();
            }
        }
        public List<Trail> GetDeactivedList()
        {
            return deactiveTrailList;
        }

        public List<Trail> GetActivatedList()
        {
            return activeTrailList;
        }

        public void DeactivetingTrail(Trail trail)
        {
            if (deactiveTrailList.Contains(trail)) return;
            activeTrailList.Remove(trail);
            deactiveTrailList.Add(trail);
            trail.transform.localPosition=Vector3.zero;
            SetPositions();
        }

        public Trail ActivetingTrail()
        {
            if (!deactiveTrailList.Any()) return null;
            Trail trail = deactiveTrailList.First();
            deactiveTrailList.Remove(trail);
            activeTrailList.Add(trail);
            SetAddedTrailPosition();
            return trail;
        }

        private void SetAddedTrailPosition()
        {
            int count = activeTrailList.Count;
            int div = count / 2;
            float currentDistance = ((distance * div) - distance / 2 * (1 - (count % 2))) * -1;
            foreach (Trail _trail in activeTrailList)
            {
                _trail.transform.localPosition = new Vector3(currentDistance, 0, 0);
                currentDistance += distance;
            }
            playerRunner.ChangeBounds(currentDistance+distance);
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