using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
        [SerializeField] private ParticleSystem firework;
        [SerializeField] private float zOffset;
        [SerializeField] private float xOffset;
        private List<ParticleSystem> fireworks=new List<ParticleSystem>();
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
            for (int i = 0; i < 2; i++)
            {
                fireworks.Add(Instantiate(firework));
                Vector3 pos = transform.position;
                pos.z += zOffset;
                if (i == 0) pos.x = xOffset;
                else pos.x = -xOffset;
                fireworks.Last().transform.position = pos;
            }
        }

        public void CollectBall() => collectedBallCount++;
        public void StartCollectingBalls()
        {
            GameManager.Instance.onEnterCheckpoint?.Invoke();
            GameManager.Instance.GetPlayerController().canMove = false;
            List<Ball> balls = BallManager.Instance.GetFloors(removeFloorCount);
            foreach (var ball in balls)
            {
                ball.StartMoveToPool();
                ball.UnregisterBall();
                ball.transform.parent = transform;
                ballCount++;
            }
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
            GameManager.Instance.onExitCheckpoint?.Invoke();
            if(BallManager.Instance.TotalBallCount<=0)yield break;
            foreach (ParticleSystem firework in fireworks)
            {
                firework.Play();
                Destroy(firework.gameObject,2f);
            }
            GameManager.Instance.GetPlayerController().StartMove();
            BallManager.Instance.currentFloor -= removeFloorCount;
        }

        private void ReturnAllBallToPool(List<Ball> balls)
        {
            foreach (var ball in balls)
            {
                ball.gameObject.SetActive(false);
                ball.transform.parent = BallPool.Instance.transform;
                ball.transform.localPosition = Vector3.zero;
                BallManager.Instance.AddTotalBallCount(-1);
            }
        }
    }
}