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
        private List<FollowedQueue> followedQueues = new List<FollowedQueue>();

        private void Start()
        {
            followedQueues.Add(Instantiate(followedQueue,this.transform));
            followedQueues[0].Initiliaze(maxRow, this);
            for(int i=0;i<testSpawnBall;i++)followedQueues[0].AddBall();
        }
    }
}