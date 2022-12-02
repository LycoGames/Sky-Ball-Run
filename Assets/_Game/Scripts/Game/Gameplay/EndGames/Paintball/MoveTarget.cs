using System;
using _Game.Scripts.Game.Gameplay.Runner;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Paintball
{
    public class MoveTarget : MonoBehaviour
    {
        public Action<int> TargetHit;
        [SerializeField] private int currentWaypoint;

        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float moveSpeed;

        // Start is called before the first frame update
        private void Start()
        {
            currentWaypoint = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            Movement();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Ball ball)) return;
            CalculatePoints(other.transform.position);
            ball.StopForward();
        }


        private void Movement()
        {
            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.3f)
            {
                currentWaypoint = GetNextPoint(currentWaypoint);
            }

            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position,
                moveSpeed * Time.deltaTime);
        }

        private int GetNextPoint(int i)
        {
            if (i + 1 == waypoints.Length)
            {
                return 0;
            }

            return i + 1;
        }

        private void CalculatePoints(Vector3 hitPosition)
        {
            var distance = Mathf.Abs(transform.position.x - hitPosition.x);

            switch (distance)
            {
                case <= 1:
                    TargetHit?.Invoke(5);
                    break;
                case <= 2:
                    TargetHit?.Invoke(4);
                    break;
                case <= 3:
                    TargetHit?.Invoke(3);
                    break;
                case <= 4:
                    TargetHit?.Invoke(2);
                    break;
                case <= 5:
                    TargetHit?.Invoke(1);
                    break;
            }
        }
    }
}