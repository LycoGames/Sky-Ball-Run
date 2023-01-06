using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.ObjectPools;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Paintball
{
    public class PaintballGame : EndGameController
    {
        [SerializeField] private PaintballWeaponController paintballWeaponController;
        [SerializeField] private float weaponSensitivity;

        [Space] [Header("PaintballWeapon")] [SerializeField]
        private PaintballWeapon paintballWeapon;

        [SerializeField] private CountTextUI countTextUI;
        [SerializeField] private ImageOverUIMoverCanvas crosshairUI;


        [SerializeField] private int ballToBulletDivider = 10;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float rateOfFire;
        [SerializeField] private float randomnessMinX;
        [SerializeField] private float randomnessMaxX;
        [SerializeField] private float randomnessMinY;
        [SerializeField] private float randomnessMaxY;


        [Space] [SerializeField] private MoveTarget target;
        [SerializeField] private Transform aimTarget;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private Camera cam;
        private int ballCount;
        private GameObject joystickPanel;

        public override void LaunchEndGame()
        {
            StartCoroutine(Launch());
        }

        public void Setup(GameObject _joystickPanel)
        {
            cam = Camera.main;
            SetupWeapon();
            SetupTarget();
            joystickPanel = _joystickPanel;
            joystickPanel.SetActive(true);
            SetupWeaponController();
        }


        private IEnumerator Launch()
        {
            yield return ReloadPaintballWeapon();
            SwitchToWeaponCamera();
            yield return new WaitForSeconds(1f);
            ActivateWeaponUIs();
            paintballWeaponController.Launch();
            yield return new WaitForSeconds(.5f);
            paintballWeapon.Fire();
        }


        private void SetupWeapon()
        {
            var bulletCount = BallManager.Instance.TotalBallCount / ballToBulletDivider;
            bulletCount = bulletCount <= 0 ? 1 : bulletCount;
            paintballWeapon.Setup(bulletSpeed, rateOfFire, bulletCount, randomnessMinX,
                randomnessMaxX, randomnessMinY, randomnessMaxY, countTextUI, EndGameEnd);
        }

        private void EndGameEnd()
        {
            StartCoroutine(EndGameCoroutine());
        }

        private IEnumerator EndGameCoroutine()
        {
            yield return new WaitForSeconds(1f);
            paintballWeaponController.StopControl();
            joystickPanel.gameObject.SetActive(false);
            EndGameEnded?.Invoke();
        }

        private void SetupWeaponController()
        {
            paintballWeaponController.Setup(weaponSensitivity, aimTarget, crosshairUI, cam);
        }

        private void SetupTarget()
        {
            target.TargetHit += GainCoin;
        }

        private IEnumerator ReloadPaintballWeapon()
        {
            foreach (var ball in BallPool.Instance.GetAllActiveBall())
            {
                ball.transform.DOMove(paintballWeapon.MagazinePosition, 1f).SetEase(Ease.Linear)
                    .OnComplete(() => { })
                    .SetAutoKill(true);
            }

            yield return new WaitForSeconds(1f);
            BallPool.Instance.ReturnAllBallToPool();
        }

        private void SwitchToWeaponCamera()
        {
            virtualCamera.Priority = 15;
        }

        private void GainCoin(int count)
        {
            GainedCoin += count;
            GainedCoinDiamond?.Invoke(GainedCoin);
            AudioSourceController.Instance.PlaySoundType(SoundType.DiamondCollected);
        }

        private void DeactivateUIs()
        {
            countTextUI.Deactivate();
            crosshairUI.Deactivate();
        }

        private void ActivateWeaponUIs()
        {
            crosshairUI.Activate();
            countTextUI.Activate();
        }
    }
}