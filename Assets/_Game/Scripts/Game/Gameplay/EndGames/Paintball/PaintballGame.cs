using System.Collections;
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

        [SerializeField] private float bulletSpeed;
        [SerializeField] private float rateOfFire;
        [SerializeField] private float randomnessMinX;
        [SerializeField] private float randomnessMaxX;
        [SerializeField] private float randomnessMinY;
        [SerializeField] private float randomnessMaxY;


        [Space] [SerializeField] private MoveTarget target;
        [SerializeField] private Transform aimTarget;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        [SerializeField] private ParticleSystem confettiEffect;


        public override void LaunchEndGame()
        {
            StartCoroutine(Launch());
        }

        public void Setup()
        {
            SetupWeapon();
            SetupTarget();
            SetupWeaponController();
        }

        private IEnumerator Launch()
        {
            ReloadPaintballWeapon();
            yield return new WaitForSeconds(1f);
            SwitchToWeaponCamera();
            yield return new WaitForSeconds(1f);
            paintballWeaponController.Launch();
            yield return new WaitForSeconds(.5f);
            paintballWeapon.Fire();
        }

        private void SetupWeapon()
        {
            paintballWeapon.Setup(bulletSpeed, rateOfFire, BallManager.Instance.TotalBallCount, randomnessMinX,
                randomnessMaxX, randomnessMinY, randomnessMaxY, EndGameEnd);
        }

        private void EndGameEnd()
        {
            StartCoroutine(EndGameCoroutine());
        }

        private IEnumerator EndGameCoroutine()
        {
            yield return new WaitForSeconds(1f);
            paintballWeaponController.StopControl();
            confettiEffect.Play();
            EndGameEnded?.Invoke();
        }

        private void SetupWeaponController()
        {
            paintballWeaponController.Setup(weaponSensitivity, aimTarget);
        }

        private void SetupTarget()
        {
            target.TargetHit += GainCoin;
        }


        private void ReloadPaintballWeapon()
        {
            foreach (var ball in BallPool.Instance.GetAllActiveBall())
            {
                ball.transform.DOMove(paintballWeapon.MagazinePosition, 1f).SetEase(Ease.Linear)
                    .OnComplete(() => ball.ReturnToPool());
            }
        }


        private void SwitchToWeaponCamera()
        {
            virtualCamera.Priority = 15;
        }

        private void GainCoin(int count)
        {
            GainedCoin += count;
            GainedCoinDiamond?.Invoke(GainedCoin);
        }
    }
}