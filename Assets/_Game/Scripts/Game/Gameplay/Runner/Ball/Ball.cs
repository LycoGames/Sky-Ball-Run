using System.Collections;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Ball
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private float waitForRemove = 1.5f;
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private float distance;
        private BallColumn ballColumn;





        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                effect.Play();
                meshRenderer.enabled = false;
                BallManager.ballManager.totalBallCount--;
                Invoke("RemoveBall",waitForRemove);
                Invoke("StartForwading",waitForRemove+0.05f);
            }
            else if (other.CompareTag("Gate"))
            {
                effect.Play();
                meshRenderer.enabled = false;
                BallManager.ballManager.totalBallCount--;
                Invoke("RemoveBall",waitForRemove);
            }
        }

        public void SetHeight(float position)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToDestination(position*distance));
        }

        public void SetBall(BallColumn _ballColumn)
        {
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
                meshRenderer.enabled = true;
                BallManager.ballManager.totalBallCount++;
            }
            SetColumn(_ballColumn);
        }

        public void SetColumn(BallColumn _ballColumn)
        {
            if(ballColumn!=null)ballColumn.UnregisterColumn(this);
            transform.parent = _ballColumn.transform;
            ballColumn = _ballColumn;
            ballColumn.RegisterColumn(this);
        }
        private void RemoveBall()
        {
            ballColumn.UnregisterColumn(this);
            gameObject.SetActive(false);
        }

        private void StartForwading()
        {
            BallManager.ballManager.StartForwading();
        }

        private IEnumerator MoveToDestination(float height)
        {
            Vector3 newPos = Vector3.zero;
            newPos.y = height;
            while (Vector3.Distance(transform.localPosition, newPos) >= 0)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, speed * Time.deltaTime);
                yield return null;
            }
            transform.localPosition = newPos;
            yield return null;
        }
    }
}