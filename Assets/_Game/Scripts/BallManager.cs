using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Player;
using UnityEngine;

namespace _Game.Scripts
{
    public class BallManager : MonoBehaviour
    {
        [SerializeField] private Followed followed;
        [SerializeField] private float distance;
        [SerializeField] private int startBall;
        [SerializeField] private int minRow;
        [SerializeField] private int minColumn;
        [SerializeField] private int minColumnAndRow;
        private int currentRow;
        private int currentColumn;
        private int currentColumnAndRow;
        [SerializeField] private int maxRow;
        [SerializeField] private int maxColumn;
        [SerializeField] private int maxColumnAndRow;
        [SerializeField] private PlayerRunner playerRunner;
        [SerializeField] private float boundHorMax = 9.6f;
        [SerializeField] private float boundHorMin = 9.6f;

        private List<Followed> activeFollowedList=new List<Followed>();
        private List<Followed> deactiveFollowedList=new List<Followed>();
        private int activeCount;

        private void Awake()
        {
            SetIndexs();
            InitiliazeFollowedList();
        }

        private void SetIndexs()
        {
            currentColumn = minColumn;
            currentRow = minRow;
            currentColumnAndRow = minColumnAndRow;
        }

        private void InitiliazeFollowedList()
        {
            for (int i = 0; i < currentColumnAndRow - activeFollowedList.Count; i++)
            {
                Followed followed = Instantiate(this.followed, transform);
                followed.Initiliaze(currentColumn, currentColumn, this);
                deactiveFollowedList.Add(followed);
            }
        }

        private void Start()
        {
            for(int i=0;i<startBall;i++)AddBall();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                collectable.OnHit(this);
            }
        }

        public void MultiplyUpper()
        {
            int totalBallCount=0;
            foreach (Followed followed in activeFollowedList)
            {
                totalBallCount += followed.GetBallCount();
            }
            AddBalls(totalBallCount);
        }

        public void MultiplyBack()
        {
            int totalBallCount=0;
            foreach (Followed followed in activeFollowedList)
            {
                totalBallCount += followed.GetBallCount();
                currentRow = Mathf.Clamp(currentRow * 2, minRow, maxRow);
                followed.Initiliaze(currentColumn,currentRow,this);
            }
            AddBalls(totalBallCount);
        }

        public void MultiplyRight()
        {
            int totalBallCount=0;
            foreach (Followed followed in activeFollowedList)
            {
                totalBallCount += followed.GetBallCount();
            }
            currentColumnAndRow=activeFollowedList.Count*2>currentColumnAndRow?activeFollowedList.Count*2:currentColumnAndRow;
            currentColumnAndRow = Math.Clamp(currentColumnAndRow, minColumnAndRow, maxColumnAndRow);
            InitiliazeFollowedList();
            AddBalls(totalBallCount);
        }
        
        public void AddBalls(int ballCount)
        {
            for(;ballCount>0;ballCount--)AddBall();
        }

        public void AddBall()
        {
            if (activeCount < currentColumnAndRow)
            {
                AddFollowed();
                activeFollowedList[activeCount-1].AddBall();
                RepositioningFollowed();
            }
            else
            {
                Followed smallestFollowed=activeFollowedList[activeCount-1];
                for (int i = activeCount - 2; i >= 0; i--)
                {
                    if (smallestFollowed.GetBallCount() >= activeFollowedList[i].GetBallCount())
                        smallestFollowed = activeFollowedList[i];
                }
                smallestFollowed.AddBall();
            }
        }
        public void RemoveFollowed(Followed followed)
        {
            if(activeCount<=0)return;
            currentColumn--;
            currentColumn = Math.Clamp(currentColumn, minColumnAndRow, maxColumnAndRow);
            activeCount--;
            deactiveFollowedList.Add(followed);
            activeFollowedList.Remove(followed);
            RepositioningFollowed();
        }
        
        private void AddFollowed()
        {
            if (!deactiveFollowedList.Any()) return;
            activeCount++;
            activeFollowedList.Add(deactiveFollowedList[0]);
            deactiveFollowedList.RemoveAt(0);
            RepositioningFollowed();
            
        }

        private void RepositioningFollowed()
        {
            float xPos = (activeCount / 2) * -distance;
            xPos -= activeCount % 2 == 0 && activeCount>1 ?  distance / 2:0;
            playerRunner.SetBounds(boundHorMax+xPos,boundHorMin-xPos);
            for (int i = 0; i < activeCount; i++)
            {
                Vector3 pos = activeFollowedList[i].transform.localPosition ;
                pos.x = xPos;
                activeFollowedList[i].transform.localPosition = pos;
                xPos += distance;
            }
            
        }

    }
}