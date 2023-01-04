using System;
using System.Collections;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Paintball
{
    public class PaintballWeaponController : MonoBehaviour
    {
        private Transform aimTarget;
        private ImageOverUIMoverCanvas crosshairUI;
        private Camera cam;
        private FixedJoystick fixedJoystick;

        private float sensitivity;
        private bool setupCompleted;

        public void Setup(float _sensitivity, Transform _aimTarget, ImageOverUIMoverCanvas _crosshairUI, Camera _cam,
            FixedJoystick _fixedJoystick)
        {
            sensitivity = _sensitivity;
            aimTarget = _aimTarget;
            crosshairUI = _crosshairUI;
            cam = _cam;
            crosshairUI.Setup(cam);
            crosshairUI.ChangeImagePosition(aimTarget.position);
            fixedJoystick = _fixedJoystick;
        }

        public void Launch()
        {
            StartCoroutine(AimingCoroutine());
            StartCoroutine(WeaponRotationCoroutine());
        }

        private IEnumerator AimingCoroutine()
        {
            while (true)
            {
                var newPos = aimTarget.position;
                newPos.y = 11.5f;
                newPos.x += fixedJoystick.Horizontal / 2;
                newPos.x = Math.Clamp(newPos.x, -10, 10);
                aimTarget.position = newPos;
                crosshairUI.ChangeImagePosition(aimTarget.position);
                yield return null;
            }
        }

        private IEnumerator WeaponRotationCoroutine()
        {
            float singleStep = sensitivity * Time.deltaTime;
            while (true)
            {
                Vector3 targetDirection = aimTarget.position - transform.position;
                if (transform.forward != targetDirection)
                {
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);
                }

                yield return null;
            }
        }

        public void StopControl()
        {
            StopAllCoroutines();
        }
    }
}