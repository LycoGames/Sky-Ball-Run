using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column;
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
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Rigidbody myRigidbody;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private float distance;
        [SerializeField] private float spawmPositionZ=10f;
        [SerializeField] private Collider collider;
        private BallColumn ballColumn;


        private void OnEnable()
        {
            collider.isTrigger = true;
            myRigidbody.isKinematic = true;
            myRigidbody.useGravity = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle")&&meshRenderer.enabled)
            {
                effect.Play();
                audioSource.Play();
                meshRenderer.enabled = false;
                Invoke("RemoveBall",waitForRemove);
                Invoke("StartForwading",waitForRemove+0.05f);
            }

        }
        
        public void StartMoveToPool()
        {
            ballColumn.UnregisterColumn(this);
            collider.isTrigger = false;
            myRigidbody.isKinematic = false;
            myRigidbody.useGravity = true;
            myRigidbody.velocity=Vector3.forward*moveForwardSpeed;
        }

        public void SetHeight(float position)
        {
            if (myRigidbody.useGravity) return;
            StopAllCoroutines();
            StartCoroutine(MoveToDestination(position*distance));
        }
        //TODO burada fazlalık olabilir.
        public void SetBall(BallColumn _ballColumn)
        {
                gameObject.SetActive(true);
                meshRenderer.enabled = true;
                SetParent(_ballColumn);
                BallManager.Instance.totalBallCount++;
                transform.rotation=Quaternion.Euler(0,0,0);
                transform.localPosition = new Vector3(0,distance*(_ballColumn.BallCount()),-spawmPositionZ);
                SetColumn(_ballColumn);
        }

       
        public void SwapColumn(BallColumn _ballColumn)
        {
            SetParent(_ballColumn);
            SetColumn(_ballColumn);
        }

        public void SetColumn(BallColumn _ballColumn)
        {
            ballColumn = _ballColumn;
            ballColumn.RegisterColumn(this);
        }
        public void RemoveBall()
        {  
            ballColumn.UnregisterColumn(this);
            gameObject.SetActive(false);
            BallManager.Instance.totalBallCount--;
        }

        private void StartForwading()
        {
            BallManager.Instance.StartForwarding();
        }

        
        private IEnumerator MoveToDestination(float height)
        {
            Vector3 newPos = Vector3.zero;
            newPos.y = height;
            while (Vector3.Distance(transform.localPosition, newPos) >= 0.02f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, speed * Time.deltaTime);
                yield return null;
            }
            transform.localPosition = newPos;
            yield return null;
        }
        private void SetParent(BallColumn _ballColumn)
        {
            transform.parent = _ballColumn.transform;
        }

    }
}