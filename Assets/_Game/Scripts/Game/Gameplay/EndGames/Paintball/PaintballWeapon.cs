using System.Collections;
using _Game.Scripts.Game.ObjectPools;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Paintball
{
    public class PaintballWeapon : MonoBehaviour
    {
        [SerializeField] private Transform barrelTransform;
        [SerializeField] private Transform magazineTransform;

        public Vector3 MagazinePosition { get; private set; }

        private int totalBallCount;
        private int firedBallCount;
        private float bulletSpeed;
        private float rateOfFire;
        private float randomnessMinX;
        private float randomnessMaxX;
        private float randomnessMinY;
        private float randomnessMaxY;

        private WaitForSeconds rateOfFireWaitForSecond;

        public void Setup(float _bulletSpeed, float _rateOfFire, int _ballCount, float _randomnessMinX,
            float _randomnessMaxX, float _randomnessMinY, float _randomnessMaxY)
        {
            bulletSpeed = _bulletSpeed;
            rateOfFire = _rateOfFire;
            totalBallCount = _ballCount;
            randomnessMinX = _randomnessMinX;
            randomnessMaxX = _randomnessMaxX;
            randomnessMinY = _randomnessMinY;
            randomnessMaxY = _randomnessMaxY;
            MagazinePosition = magazineTransform.position;
            rateOfFireWaitForSecond = new WaitForSeconds(rateOfFire);
        }

        public void Fire()
        {
            StartCoroutine(FireCoroutine());
        }

        private IEnumerator FireCoroutine()
        {
            while (firedBallCount <= totalBallCount)
            {
                var ball = BallPool.Instance.GetPooledObject();
                ball.transform.position = barrelTransform.position;
                ball.transform.rotation = barrelTransform.rotation;
                ball.SetActive(true);
                var rb = ball.gameObject.AddComponent<Rigidbody>();
                rb.velocity = barrelTransform.forward * bulletSpeed + GetRandomRecoil();
                firedBallCount++;
                yield return rateOfFireWaitForSecond;
            }
        }

        private Vector3 GetRandomRecoil()
        {
            return new Vector3(Random.Range(randomnessMinX, randomnessMaxX),
                Random.Range(randomnessMinY, randomnessMaxY), 0);
        }
    }
}