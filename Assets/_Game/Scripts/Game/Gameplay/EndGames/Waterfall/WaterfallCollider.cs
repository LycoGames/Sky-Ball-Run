using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Waterfall
{
    public class WaterfallCollider : MonoBehaviour
    {
        [SerializeField] private float zMin;
        [SerializeField] private float zMax;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Ball")) return;
            Rigidbody rb;
            rb = other.TryGetComponent(out Rigidbody rigidbody)
                ? rigidbody
                : other.gameObject.AddComponent<Rigidbody>();
            rb.mass = 1000f;
            float zPos = Mathf.Clamp(other.transform.position.z, zMin, zMax);
            other.transform.parent = null;
            Vector3 newPos = other.transform.position;
            newPos.z = zPos;
            other.transform.position = newPos;
            other.isTrigger = false;
            rb.velocity = new Vector3(other.transform.position.x * 2, 0, 0);
        }
    }
}