using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Player;
using UnityEngine;

namespace _Game.Scripts
{
    public class BallManager : MonoBehaviour
    {
        [SerializeField] private FollowedQueue followedQueue;
        [SerializeField] private int maxRow = 99;
        [SerializeField] private int testSpawnBall = 20;
        [SerializeField] private int testSpawnFollowedQueue = 5;
        [SerializeField] private float distance = 0.5f;
        private int followedQueueCount;
        private List<FollowedQueue> followedQueues = new List<FollowedQueue>();

        private void Start()
        {
            for (int i = 0; i < testSpawnFollowedQueue; i++)
            {
                followedQueues.Add(Instantiate(followedQueue,this.transform));
                if(i==0)followedQueues[i].Initiliaze(maxRow, this,null);
                else followedQueues[i].Initiliaze(maxRow, this,followedQueues[i-1]);
                followedQueues[i].transform.localPosition = new Vector3(0,distance*(testSpawnFollowedQueue-1-i), 0);
                for(int j=0;j<testSpawnBall;j++)followedQueues[i].AddBall();
                //
            }
        }
        
    }
}