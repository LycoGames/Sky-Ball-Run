using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Paintball
{
    public class MoveTarget : MonoBehaviour
    {
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
    }
}