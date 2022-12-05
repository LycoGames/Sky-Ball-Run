using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner;
using _Game.Scripts.Game.ObjectPools;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Game.Gameplay.EndGames.Paintball
{
    public class PaintballWeapon : MonoBehaviour
    {
        [SerializeField] private Transform barrelTransform;
        [SerializeField] private Transform magazineTransform;

        private Action OnOutOfBullet;

        public Vector3 MagazinePosition { get; private set; }

        private int totalBallCount;
        private int firedBallCount;
        private float bulletSpeed;
        private float rateOfFire;
        private float randomnessMinX;
        private float randomnessMaxX;
        private float randomnessMinY;
        private float randomnessMaxY;
        private CountTextUI countTextUI;

        private WaitForSeconds rateOfFireWaitForSecond;

        public void Setup(float _bulletSpeed, float _rateOfFire, int _ballCount, float _randomnessMinX,
            float _randomnessMaxX, float _randomnessMinY, float _randomnessMaxY, CountTextUI countTextUI,
            Action _onOutOfBullet)
        {
            this.countTextUI = countTextUI;
            bulletSpeed = _bulletSpeed;
            rateOfFire = _rateOfFire;
            totalBallCount = _ballCount;
            this.countTextUI.ChangeCount(totalBallCount);
            randomnessMinX = _randomnessMinX;
            randomnessMaxX = _randomnessMaxX;
            randomnessMinY = _randomnessMinY;
            randomnessMaxY = _randomnessMaxY;
            MagazinePosition = magazineTransform.position;
            rateOfFireWaitForSecond = new WaitForSeconds(rateOfFire);
            OnOutOfBullet = _onOutOfBullet;
        }

        public void Fire()
        {
            StartCoroutine(FireCoroutine());
        }

        private IEnumerator FireCoroutine()
        {
            while (firedBallCount < totalBallCount)
            {
                var ball = BallPool.Instance.GetPooledObject();
                ball.transform.position = barrelTransform.position;
                ball.transform.rotation = barrelTransform.rotation;
                ball.gameObject.SetActive(true);
                var ballSc = ball.GetComponent<Ball>();
                ballSc.StartCoroutine(ballSc.MoveToForward(bulletSpeed, GetRandomRecoil()));
                // Rigidbody rb;
                // rb = ball.TryGetComponent(out Rigidbody rigidbody)
                //     ? rigidbody
                //     : ball.gameObject.AddComponent<Rigidbody>();
                // rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                // rb.velocity = barrelTransform.forward * bulletSpeed + GetRandomRecoil();
                firedBallCount++;
                UpdateBulletCountUI(totalBallCount - firedBallCount);
                yield return rateOfFireWaitForSecond;
            }

            OnOutOfBullet?.Invoke();
        }

        private Vector3 GetRandomRecoil()
        {
            return new Vector3(Random.Range(randomnessMinX, randomnessMaxX),
                Random.Range(randomnessMinY, randomnessMaxY), 0);
        }

        private void UpdateBulletCountUI(int value)
        {
            countTextUI.ChangeCount(value);
        }
    }
}