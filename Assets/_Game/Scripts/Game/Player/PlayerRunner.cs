using System;
using System.Collections;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerRunner : MonoBehaviour
    {
        //[SerializeField] private FloatingJoystick joystick;
        [SerializeField] public float movementSpeed = 10;
        [SerializeField] private float rotationSpeed = 10;
        [SerializeField] private float boundHorMax = 9.6f;
        [SerializeField] private float boundHorMin = 9.6f;
        [SerializeField] private float maxRotationDegree = 20f;
        [SerializeField] private Rigidbody rb;
        private Vector3 startPos;
        private Rigidbody _theRb;

        void Awake()
        {
            SetRigidbody();
        }

        private void OnEnable()
        {
            GetStartPosition();
        }

        private void OnDisable()
        {
            ResetStartPosition();
        }

        private void ResetStartPosition()
        {
            transform.position = startPos;
        }

        private void GetStartPosition()
        {
            startPos = transform.position;
        }

        private void SetRigidbody()
        {
            _theRb = rb;
        }

        private void FixedUpdate()
        {
            RotateY(Input.GetAxis("Vertical"),Input.GetAxis("Horizontal"));
            Movement();
        }

        private float xAngle=0;
        private float Altitude(float rotateTo)
        {
            float yPos = transform.position.y;
            if (yPos == 0.5f && xAngle > 0)
            {
                xAngle -= rotationSpeed * Time.deltaTime;
                if (xAngle < 0) xAngle = 0;
            }
            else if (rotateTo > 0)
            {
                xAngle -= rotateTo * rotationSpeed * Time.deltaTime;
            }
            else if(yPos>0.5f)
            {
                xAngle += rotationSpeed * Time.deltaTime;
            }

            return xAngle;

        }

        
        private void RotateY(float rotateXTo,float rotateYTo)
        {
            _theRb.MoveRotation(Quaternion.Euler(Altitude(rotateXTo), RotateAngle(rotateYTo), 0));
        }
        private float yAngle = 0;
        private float RotateAngle(float rotateTo)
        {
            float xPos = transform.position.x;
            if ((xPos == boundHorMax && rotateTo>0)||(xPos == boundHorMin&&rotateTo<0)) rotateTo = 0;
            if (rotateTo != 0)
            {
                yAngle=Math.Clamp(yAngle+rotationSpeed*rotateTo*Time.deltaTime,-maxRotationDegree, maxRotationDegree);
            }
            else if (yAngle > 0)
            {
                yAngle -= rotationSpeed*Time.deltaTime;
                if (yAngle < 0) yAngle = 0;
            }
            else if (yAngle < 0)
            {
                yAngle += rotationSpeed*Time.deltaTime;
                if (yAngle > 0) yAngle = 0;
            }
            return yAngle;
        }

        private void Movement()
        {
            var pos = transform.position + transform.forward * movementSpeed * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, boundHorMin, boundHorMax);
            pos.y = Math.Clamp(pos.y, 0.5f, 100);
            _theRb.MovePosition(pos);
        }
        
        
    }
}

