using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class BallMover : MonoBehaviour
    {

        [SerializeField] private float speed;
        [SerializeField] private float yBound=-6.5f;
        [SerializeField] private float xBound=9.3f;
        [SerializeField] private float zMinBound=-10f;
        [SerializeField] private float zMaxBound=30f;
        [SerializeField] private float bounceAngle = 15f;
        [SerializeField] private float rotationXSpeed;
        private bool dropState;
        private float xAngle;
        private float yAngle;
        private float currentBounce;
        private float currentSpeed;
     

        private void OnEnable()
        {
            currentSpeed = speed;
            dropState = false;
        }

        private void OnDisable()
        {
            dropState = false;
        }

        private void Update()
        {
            MoveForward();
            if (dropState)
            {
                Rotate();
            } 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BallDropper"))
            {
                dropState = true;
                xAngle = 0;
                yAngle = Random.Range(-15, 15);
            }
        }

        public void OnDropState()
        { 
            dropState = true;
        }

        private void MoveForward()
        {
            transform.localPosition += transform.forward * currentSpeed * Time.deltaTime;
            if (dropState)
            {
                float x = transform.localPosition.x;
                float z = transform.localPosition.z;
                x = Mathf.Clamp(x, -xBound, xBound);
                if (z >= zMaxBound || z <= zMinBound)
                {
                    yAngle *= -180;
                    currentSpeed *= 0.8f;
                }
                z = Mathf.Clamp(z, zMinBound, zMaxBound);
                transform.localPosition = new Vector3(x, transform.localPosition.y, z);
            }
        }

        private void Rotate()
        {
            transform.rotation = Quaternion.Euler(UpDownAngle(), yAngle, 0);
        }
    
        private float UpDownAngle()
        {
            float yPos = transform.localPosition.y;
            if (yPos <= yBound && xAngle > 0)
            {
                if (xAngle > bounceAngle)
                {
                    xAngle *= -1;
                    xAngle /= 2;
                    xAngle += bounceAngle;
                }
                else
                {
                    xAngle = 0;
                }
            }
            else if (yPos > yBound)
            {
                xAngle = Mathf.Clamp(xAngle + rotationXSpeed * Time.deltaTime, -80f, 80f);
            }

            return xAngle;
        }
    }
}
