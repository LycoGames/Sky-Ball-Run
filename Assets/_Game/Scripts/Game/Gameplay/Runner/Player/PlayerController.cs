using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 10;
        [SerializeField] private float rotationYSpeed = 10;
        [SerializeField] private float rotationXSpeed = 10;
        [SerializeField] private float boundHorizontal = 9.6f;
        [SerializeField] private float maxRotationDegree = 20f;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float bounceAngle = 15f;
        private float orginalBound;

        private Vector3 startPos;
        private float xAngle; //Todo bunlardan kurtul
        private float yAngle;

        public float VerticalInput;
        public float HorizontalInput;
        public bool canMove=false;
        private void OnEnable()
        {
            GetStartPosition();
            orginalBound = boundHorizontal;
            GameManager.Instance.onStartGame += StartMove;
        }

        private void OnDisable()
        {
            ResetStartPosition();
            GameManager.Instance.onStartGame -= StartMove;
        }

        private void FixedUpdate()
        {
            if(canMove)Movement();
            Rotate(VerticalInput, HorizontalInput);
        }

        public void StartMove()
        {
            canMove = true;
        }

        public void StopMove()
        {
            canMove = false;
        }

        public void ChangeBounds(float changeValue)
        {
            boundHorizontal = orginalBound - changeValue;
        }

        private void Movement()
        {
            var pos = transform.position + transform.forward;
            pos.x = Mathf.Clamp(pos.x, -boundHorizontal, boundHorizontal);
            pos.y = Math.Clamp(pos.y, 0, 100);
            transform.position = Vector3.MoveTowards(transform.position, pos, movementSpeed * Time.deltaTime);
        }

        private void Rotate(float rotateXTo, float rotateYTo)
        {
            transform.rotation = Quaternion.Euler(0, LeftRightAngle(rotateYTo), 0);
        }

        private float UpDownAngle(float rotateTo)
        {
            float yPos = transform.position.y;
            if (yPos <= 0.01f && xAngle > 0)
            {
                if (xAngle > bounceAngle)
                {
                    xAngle *= -1;
                    xAngle += bounceAngle;
                }
                else if (xAngle < bounceAngle) xAngle -= rotationXSpeed * 2 * Time.deltaTime;
                else if (xAngle < 0) xAngle = 0;
            }
            else if (rotateTo > 0)
            {
                xAngle = Math.Clamp(xAngle - rotationXSpeed * Time.deltaTime, -45f, 45f);
            }
            else if (yPos > 0f)
            {
                xAngle = Math.Clamp(xAngle + rotationXSpeed * Time.deltaTime, -45f, 45f);
            }

            return xAngle;
        }

        private float LeftRightAngle(float rotateTo)
        {
            rotateTo = Math.Clamp(rotateTo, -1, 1);
            float xPos = transform.position.x;
            if ((xPos == boundHorizontal && rotateTo > 0) || (xPos == -boundHorizontal && rotateTo < 0)) rotateTo = 0;
            if (rotateTo != 0)
            {
                yAngle = Math.Clamp(yAngle + rotationYSpeed * rotateTo * Time.deltaTime, -maxRotationDegree,
                    maxRotationDegree);
            }
            else
            {
                yAngle = RotateToOriginal(yAngle, rotationYSpeed);
            }

            return yAngle;
        }

        private float RotateToOriginal(float angle, float speed)
        {
            if (angle > 0)
            {
                angle -= speed * Time.deltaTime;
                if (angle < 0) angle = 0;
            }
            else if (angle < 0)
            {
                angle += speed * Time.deltaTime;
                if (angle > 0) angle = 0;
            }

            return angle;
        }

        private void ResetStartPosition()
        {
            transform.position = startPos;
        }

        private void GetStartPosition()
        {
            startPos = transform.position;
        }
    }
}