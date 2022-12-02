using System;
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
        public Action<int> TargetHit;

        [Header("PaintballWeapon")] [SerializeField]
        private PaintballWeapon paintballWeapon;

        [SerializeField] private float bulletSpeed;
        [SerializeField] private float rateOfFire;
        [SerializeField] private float randomnessMinX;
        [SerializeField] private float randomnessMaxX;
        [SerializeField] private float randomnessMinY;
        [SerializeField] private float randomnessMaxY;


        [Space] [SerializeField] private MoveTarget target;

        [SerializeField] private CinemachineVirtualCamera
            virtualCamera;

        public override void LaunchEndGame()
        {
            SetupWeapon();
            SetupTarget();
            StartCoroutine(Launch());
        }


        private IEnumerator Launch()
        {
            ReloadPaintballWeapon();
            yield return new WaitForSeconds(1f);
            SwitchToWeaponCamera();
            yield return new WaitForSeconds(1f);
            paintballWeapon.Fire();
        }

        private void SetupWeapon()
        {
            paintballWeapon.Setup(bulletSpeed, rateOfFire, BallManager.Instance.TotalBallCount, randomnessMinX,
                randomnessMaxX, randomnessMinY, randomnessMaxY);
        }

        private void SetupTarget()
        {
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
    }
}