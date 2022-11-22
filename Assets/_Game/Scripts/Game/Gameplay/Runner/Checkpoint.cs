using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.ObjectPools;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class Checkpoint : MonoBehaviour
    {
        public List<Transform> transforms = new List<Transform>();
        [SerializeField] private Barricade barricade;
        [SerializeField] private Transform moverLine;
        [SerializeField] private int removeFloorCount = 2;
        [SerializeField] private float coverCloseTime = 2f;
        private int ballCount;
        private int collectedBallCount;

        private void Start()
        {
            foreach (Transform _transform in transform.parent.GetComponentsInChildren<Transform>())
            {
                if (_transform == transform.parent || CheckIsChild(_transform)) continue;
                transforms.Add(_transform);
            }

            foreach (Transform _transform in transforms)
            {
                _transform.gameObject.SetActive(false);
            }

            barricade = Instantiate(barricade, transform.parent);
        }

        public void CollectBall() => collectedBallCount++;

        public void StartCollectingBalls()
        {
            GameManager.Instance.GetPlayerController().canMove = false;
            List<Ball> balls = BallManager.Instance.GetFloors(removeFloorCount);
            foreach (Ball ball in balls)
            {
                ball.StartMoveToPool();
                ball.transform.parent = transform;
            }

            ballCount = balls.Count;
            StartCoroutine(OnAllBallCollected(balls));
        }

        private bool CheckIsChild(Transform child)
        {
            return transform.GetComponentsInChildren<Transform>().Any(_transform => child == _transform);
        }

        private IEnumerator OnAllBallCollected(List<Ball> balls)
        {
            while (ballCount > collectedBallCount) yield return null;
            barricade.OpenBarricades();
            moverLine.transform.DOLocalMoveZ(0, coverCloseTime);
            yield return new WaitForSeconds(coverCloseTime);
            moverLine.transform.DOLocalMoveY(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            ReturnAllBallToPool(balls);
            gameObject.SetActive(false);
            foreach (Transform transform in transforms)
            {
                transform.gameObject.SetActive(true);
            }

            GameManager.Instance.GetPlayerController().StartMove();
            BallManager.Instance.currentFloor -= removeFloorCount;
            BallManager.Instance.totalBallCount -= balls.Count;
        }

        private void ReturnAllBallToPool(List<Ball> balls)
        {
            foreach (Ball ball in balls)
            {
                ball.gameObject.SetActive(false);
                ball.transform.parent = BallPool.Instance.transform;
                ball.transform.localPosition = Vector3.zero;
            }
        }
    }
}