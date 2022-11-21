using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning.Column;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private float waitForRemove = 1.5f;
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private float distance;
        [SerializeField] private float spawmPositionZ=10f;
        private BallColumn ballColumn;
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle")&&meshRenderer.enabled)
            {
                effect.Play();
                meshRenderer.enabled = false;
                Invoke("RemoveBall",waitForRemove);
                Invoke("StartForwading",waitForRemove+0.05f);
            }
            else if (other.CompareTag("Gate")&&meshRenderer.enabled)
            {
                effect.Play();
                meshRenderer.enabled = false;
                Invoke("RemoveBall",waitForRemove);
            }
        }

        public void SetHeight(float position)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToDestination(position*distance));
        }
        //TODO burada fazlalık olabilir.
        public void SetBall(BallColumn _ballColumn)
        {
                gameObject.SetActive(true);
                meshRenderer.enabled = true;
                BallManager.Instance.totalBallCount++;
                SetColumn(_ballColumn);
                transform.localPosition = new Vector3(0,distance*(_ballColumn.BallCount()-1),-spawmPositionZ);
        }

        public void SwapColumn(BallColumn _ballColumn)
        {
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
            transform.position = Vector3.zero;
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