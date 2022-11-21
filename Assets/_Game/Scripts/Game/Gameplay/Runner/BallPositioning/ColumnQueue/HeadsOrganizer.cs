using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Gameplay.Runner.BallPositioning.ColumnQueue
{
    public class HeadsOrganizer : MonoBehaviour
    {
        public List<ColumnHead> ColumnHeads { get; private set; }
        public List<ColumnHead> lastActiveList = new List<ColumnHead>();

        [SerializeField] private ColumnHead columnHead;

        private PlayerController playerController;
        private float distance;

        public IEnumerator InitializeHeadsOrganizer(int maxColumn, float _distance, PlayerController playerController,
            int maxFloor, int maxRow)
        {
            this.playerController = playerController;
            distance = _distance;
            ColumnHeads = new List<ColumnHead>();
            for (int i = 0; i < maxColumn; i++)
            {
                ColumnHeads.Add(Instantiate(columnHead, transform));
                yield return StartCoroutine(ColumnHeads.Last().InitializeColumnHead(maxRow, distance, maxFloor));
            }
        }

        public void SetPositions()
        {
            List<ColumnHead> activeList = GetActiveList();
            if (ListChecker(activeList)) return;
            int count = activeList.Count;
            int div = count / 2;
            float currentDistance = ((distance * div) - distance / 2 * (1 - (count % 2))) * -1;
            for (int i = 0; i < count; i++)
            {
                activeList[i].SetPosition(currentDistance);
                currentDistance += distance;
            }
            lastActiveList = activeList;
            activeList.AddRange(ColumnHeads);
            activeList=activeList.Distinct().ToList();
            ColumnHeads.Clear();
            ColumnHeads.AddRange(activeList);
            playerController.ChangeBounds(currentDistance + distance);
            BallManager.Instance.currentColumn = count;
        }
        

        public List<ColumnHead> GetActiveList()
        {
            List<ColumnHead> activeList = new List<ColumnHead>();
            foreach (ColumnHead _columnHead in ColumnHeads)
            {
                if (_columnHead.CheckIsActive()) activeList.Add(_columnHead);
            }

            return activeList;
        }

        public IEnumerator SetPositionsInstantly()
        {
            var activeList = GetActiveList();
            int count = activeList.Count;
            int div = count / 2;
            float currentDistance = ((distance * div) - distance / 2 * (1 - (count % 2))) * -1;
            Vector3 newPos = Vector3.zero;
            lastActiveList.Clear();
            for (int i = 0; i < count; i++)
            {
                newPos.x = currentDistance;
                ColumnHeads[i].transform.localPosition = newPos;
                ColumnHeads[i].CheckIsActive();
                lastActiveList.Add(ColumnHeads[i]);
                currentDistance += distance;
                yield return null;
            }
            playerController.ChangeBounds(currentDistance + distance);
            yield return null;
        }

        private bool ListChecker(List<ColumnHead> currentList)
        {
            int listSize = currentList.Count();
            if (listSize != lastActiveList.Count) return false;
            for (int i = 0; i < listSize; i++)
            {
                if (currentList[i] != lastActiveList[i]) return false;
            }
            return true;
        }
    }
}