using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Waterfall
{
    public class WaterfallCollider : MonoBehaviour
    {
        [SerializeField] private Transform zMinPoint;
        [SerializeField] private Transform zMaxPoint;
        [SerializeField] private AudioClip clip;

        public bool GameStarted { get; set; } = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!GameStarted || !other.CompareTag("Ball")) return;
            other.GetComponent<AudioSource>().clip = clip;
            Rigidbody rb;
            rb = other.TryGetComponent(out Rigidbody rigidbody)
                ? rigidbody
                : other.gameObject.AddComponent<Rigidbody>();
            rb.mass = 1000f;
            float zPos = Mathf.Clamp(other.transform.position.z, zMinPoint.position.z, zMaxPoint.position.z);
            other.transform.parent = null;
            Vector3 newPos = other.transform.position;
            newPos.z = zPos;
            other.transform.position = newPos;
            other.isTrigger = false;
            rb.velocity = new Vector3(other.transform.position.x * 2, 0, 0);
        }
    }
}