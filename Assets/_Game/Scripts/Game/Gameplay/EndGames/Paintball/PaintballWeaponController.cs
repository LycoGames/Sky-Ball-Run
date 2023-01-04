using System;
using System.Collections;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Paintball
{
    public class PaintballWeaponController : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] private Transform moveTarget;

        private Transform aimTarget;
        private ImageOverUIMoverCanvas crosshairUI;
        private Camera cam;
        private FixedJoystick fixedJoystick;

        private float sensitivity;
        private const float Range = 70f;
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
                // if (Input.GetMouseButton(0))
                // {
                //     RaycastHit hit;
                //     Vector3 point = new Vector3();
                //     Vector2 mousePos = new Vector2();
                //     mousePos.x = Input.mousePosition.x;
                //     mousePos.y = Input.mousePosition.y;
                //     point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
                //     var dir = (point - cam.transform.position).normalized;
                //     var ray = new Ray(cam.transform.position, dir * 100f);
                //     if (Physics.Raycast(ray, out hit, Range, layerMask))
                //     {
                //         var target = hit.point;
                //         target.y = 16f;
                //         aimTarget.position = target;
                //         crosshairUI.ChangeImagePosition(aimTarget.position);
                //     }
                // }
                var newPos = aimTarget.position;
                newPos.y = 11.5f;
                newPos.x += fixedJoystick.Horizontal / 2;
                newPos.x = Math.Clamp(newPos.x, -10, 10);
                aimTarget.position = newPos;
                crosshairUI.ChangeImagePosition(aimTarget.position);

                // aimTarget.position = moveTarget.position; AIM HACK
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