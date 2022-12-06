using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column;
using _Game.Scripts.Game.ObjectPools;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class Ball : MonoBehaviour
    {
        //TODO top sütün ilişki yöntemleri düzenlenmeli

        [SerializeField] private float speed = 1;
        [SerializeField] private float moveForwardSpeed = 1;
        [SerializeField] private float waitForRemove = 1.5f;
        [SerializeField] private ParticleSystem effect;

        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private float distance;
        [SerializeField] private float spawmPositionZ = 10f;
        [SerializeField] private Collider myCollider;
        private BallColumn ballColumn;
        private Rigidbody myRigidbody;

        private void OnEnable()
        {
            myCollider.isTrigger = true;//
        }

        private void OnDisable()
        {
            Destroy(myRigidbody);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle") && meshRenderer.enabled)
            {
                RemoveBallWithAnimation();
            }
        }

        public IEnumerator MoveToForward(float speed, Vector3 randomRecoil)
        {
            while (true)
            {
                transform.position += transform.forward * (speed * Time.deltaTime);
                yield return null;
            }
        }

        public void StopForward()
        {
            StopAllCoroutines();
            meshRenderer.enabled = false;
            effect.Play();
            myCollider.enabled = false;
        }

        public void RemoveBallWithAnimation()
        {
            effect.Play();
            AudioSourceController.Instance.PlaySoundType(SoundType.BallExplode);
            meshRenderer.enabled = false;
            Invoke("RemoveBall", waitForRemove);
            Invoke("StartForwading", waitForRemove + 0.05f);
        }

        public void StartMoveToPool(float dropPosZ)
        {
            StartCoroutine(MoveToForward(dropPosZ));
        }


        public void SetHeight(float position)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToDestination(position * distance));
        }

        //TODO burada fazlalık olabilir.
        public void SetBall(BallColumn _ballColumn)
        {
            gameObject.SetActive(true);
            meshRenderer.enabled = true;
            SetParent(_ballColumn);
            BallManager.Instance.AddTotalBallCount(1);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localPosition = new Vector3(0, distance * (_ballColumn.BallCount()), -spawmPositionZ);
            SetColumn(_ballColumn);
        }

        public void SwapColumn(BallColumn _ballColumn)
        {
            SetParent(_ballColumn);
            SetColumn(_ballColumn);
        }

        public void SetColumn(BallColumn _ballColumn)
        {
            if (ballColumn != null) ballColumn.UnregisterColumn(this);
            ballColumn = _ballColumn;
            ballColumn.RegisterColumn(this);
        }

        public void RemoveBall()
        {
            UnregisterBall();
            meshRenderer.enabled = true;
            ReturnToPool();
            BallManager.Instance.AddTotalBallCount(-1);
        }

        public void UnregisterBall()
        {
            ballColumn.UnregisterColumn(this);
        }

        public void ReturnToPool()
        {
            ballColumn = null;
            gameObject.SetActive(false);
            StopAllCoroutines();
            transform.parent = BallPool.Instance.transform;
        }

        private void StartForwading()
        {
            BallManager.Instance.StartForwarding();
        }


        private IEnumerator MoveToDestination(float height)
        {
            Vector3 newPos = Vector3.zero;
            newPos.y = height;
            while (Vector3.Distance(transform.localPosition, newPos) >= 0.1f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, speed * Time.deltaTime);
                yield return null;
            }

            transform.localPosition = newPos;
            yield return null;
        }

        private IEnumerator MoveToForward(float z)
        {
            Vector3 newPos = transform.position;
            newPos.z = z;
            while (z - transform.position.z >= 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
                yield return null;
            }

            StartDroping();
            yield return null;
        }

        private void StartDroping()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            myCollider.isTrigger = false;
            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.mass = 10;
            myRigidbody.velocity = Vector3.forward * moveForwardSpeed;
        }

        private void SetParent(BallColumn _ballColumn)
        {
            transform.parent = _ballColumn.transform;
        }
    }
}