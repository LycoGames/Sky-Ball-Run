using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                ColumnHeads.Last().InitializeColumnHead(maxRow, distance, maxFloor);
            }

            yield return null;
        }

        public void SetPositions()
        {
            List<ColumnHead> activeList = new List<ColumnHead>();
            foreach (ColumnHead _columnHead in ColumnHeads)
            {
                if (_columnHead.CheckIsActive()) activeList.Add(_columnHead);
            }
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
            playerController.ChangeBounds(currentDistance + distance);
        }

        public void SetPositionsInstantly()
        {
            List<ColumnHead> activeList = new List<ColumnHead>();
            
            foreach (ColumnHead _columnHead in ColumnHeads)
            {
                if (_columnHead.CheckIsActive()) activeList.Add(_columnHead);
            }
            if (ListChecker(activeList)) lastActiveList = activeList;
            int count = activeList.Count;
            int div = count / 2;
            float currentDistance = ((distance * div) - distance / 2 * (1 - (count % 2))) * -1;
            Vector3 newPos = Vector3.zero;
            for (int i = 0; i < count; i++)
            {
                newPos.x = currentDistance;
                activeList[i].transform.localPosition = newPos;
                currentDistance += distance;
            }

            lastActiveList = activeList;
            playerController.ChangeBounds(currentDistance + distance);
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