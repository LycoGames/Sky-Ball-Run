using System;
using System.Collections;
using UnityEngine;

namespace _Game.Scripts.Game.Runner.Ball
{
    public class RemoveProcess : MonoBehaviour
    {
        [SerializeField] private float distance = 20f;

        public bool isFollow;

        private TrailManager trailManager;
        private Transform follow;


        public void Initialize(TrailManager _trailManager)
        {
            trailManager = _trailManager;
            follow = trailManager.transform;
        }


        private void FixedUpdate()
        {
            if (isFollow) transform.position = new Vector3(0, 0, follow.position.z - distance);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle")&&isFollow)
            {
                isFollow = false;
                trailManager.StartRemoveProcess();
            }
        }
    }
}