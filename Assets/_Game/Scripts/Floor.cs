using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts
{
    public class Floor : MonoBehaviour
    {
        [FormerlySerializedAs("followed")] [SerializeField] private FollowedQueue followedQueue;
        [SerializeField] private float distance = 0.1f;
        [SerializeField] private float positioningSpeed = 0.5f;
        private Dictionary<bool, List<FollowedQueue>> followedList = new Dictionary<bool, List<FollowedQueue>>();
        private int currentColumnIndex;

        // public int BallCount()
        // {
        //     int ballCount = 0;
        //     foreach (Followed followed in followedList[true])
        //     {
        //         ballCount += followed.GetBallCount();
        //     }
        //
        //     return ballCount; //TODO daha temiz olabilir.
        // }
        //
        // public void Initiliaze()
        // {
        //     InitializeFollowedList();
        //     InstantiateFollowed(BallManager.ballManager.GetMaxColumn());
        // }
        //
        // public bool AddBall()
        // {
        //      if (BallCount() == BallManager.ballManager.GetMaxColumn() * (BallManager.ballManager.GetMaxRow()))
        //          return false;
        //     if (followedList[true].Count <= currentColumnIndex)
        //     {
        //         if (!followedList[false].Any()) return false;
        //         EnableFollowed(followedList[false][0]);
        //     }
        //     CheckCurrentColumn();
        //     Followed followed = followedList[true][currentColumnIndex];
        //     if (followed.GetBallCount() >= BallManager.ballManager.GetMaxRow())
        //     {
        //         currentColumnIndex++;
        //         if (currentColumnIndex >= BallManager.ballManager.GetMaxColumn())
        //             currentColumnIndex = 0; //todo çok kötü duruyor gibi
        //         return AddBall();
        //     }
        //
        //     followed.AddBall();
        //     currentColumnIndex++;
        //     if (currentColumnIndex >= followedList[false].Count + followedList[true].Count) currentColumnIndex = 0;
        //     return true;
        // }
        //
        // private void RePositioning()
        // {
        //     int followedCount = followedList[true].Count;
        //     int div = followedCount / 2;
        //     float posOffset = 0;
        //     if (followedCount % 2 == 0)
        //     {
        //         posOffset -= distance / 2;
        //         div--;
        //     }
        //     posOffset -= div * distance;
        //     for (int i = 0; i < followedCount; i++)
        //     {
        //         //StartCoroutine(moveFollowed(followedList[true][i].transform, new Vector3(posOffset, 0, 0)));
        //         followedList[true][i].transform.localPosition = new Vector3(posOffset, 0, 0);
        //         posOffset += distance;
        //     }
        // }
        //
        //
        // IEnumerator moveFollowed(Transform followed, Vector3 newPos)
        // {
        //     while (Vector3.Distance(followed.transform.localPosition, newPos) > 0.01f)
        //     {
        //         followed.localPosition = Vector3.MoveTowards(followed.localPosition, newPos, positioningSpeed);
        //         yield return null;
        //     }
        //
        //     Debug.ClearDeveloperConsole();
        //     Debug.Log(newPos.x);
        //     followed.localPosition = newPos;
        // }
        //
        // public void EnableFollowed(Followed followed)
        // {
        //     followedList[true].Add(followed);
        //     followedList[false].Remove(followed);
        //     RePositioning();
        // }
        //
        // public void DisableFollowed(Followed followed)
        // {
        //     followedList[true].Remove(followed);
        //     followedList[false].Add(followed);
        //     if(!followedList[true].Any())BallManager.ballManager.DisableFloor(this);
        //     RePositioning();
        // }
        //
        // private void CheckCurrentColumn()
        // {
        //     int count = followedList[true].Count;
        //     if (count <= currentColumnIndex) currentColumnIndex = count - 1;
        // }
        // private void InstantiateFollowed(int followedCount)
        // {
        //     for (int i = 0; i < followedCount; i++)
        //     {
        //         Followed followeded = Instantiate(followed, transform);
        //         followeded.Initialize(this);
        //         followedList[false].Add(followeded);
        //     }
        // }
        //
        // private void InitializeFollowedList()
        // {
        //     followedList.Add(true, new List<Followed>());
        //     followedList.Add(false, new List<Followed>());
        // }
    }
}