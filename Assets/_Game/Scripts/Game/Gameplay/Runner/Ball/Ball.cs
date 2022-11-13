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
        private BallColumn ballColumn;
        private float height;



       

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                effect.Play();
                meshRenderer.enabled = false;
                RemoveBall();
            }
        }

        private void RemoveBall()
        {
            ballColumn.RemoveBall(this);
            //gameObject.SetActive(false);
        }

        public void SetHeight(float _height)
        {
            height = _height;
            StopAllCoroutines();
            StartCoroutine(MoveToDestination());
        }

        public void SetBall(BallColumn _ballColumn, float _height)
        {
            meshRenderer.enabled = true;
            ballColumn = _ballColumn;
            SetHeight(_height);
        }

        private IEnumerator MoveToDestination()
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