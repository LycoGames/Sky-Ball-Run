using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.ObjectPools;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class Checkpoint : MonoBehaviour
    {
        public List<Transform> transforms = new List<Transform>();
        [SerializeField] private Barricade barricade;
        [SerializeField] private Transform moverLine;
        [SerializeField] private int removePercentage=1;
        [SerializeField] private float coverCloseTime = 2f;
        [SerializeField] private ParticleSystem firework;
        [SerializeField] private float zOffset;
        [SerializeField] private float xOffset;
        [SerializeField] private TextMeshProUGUI removeSizeText;
        [SerializeField] private float checkTime=1f;
        private List<ParticleSystem> fireworks=new List<ParticleSystem>();
        private int removeSize;
        private int collectedBallCount;
        private BallManager ballManager;
        private WaitForSeconds wfsForCheckSize;
        private Coroutine checkSizeCoroutine;
        private void OnEnable()
        {
            ballManager=BallManager.Instance;
            checkSizeCoroutine=StartCoroutine(CheckSize());
        }

        private void Start()
        {
            wfsForCheckSize = new WaitForSeconds(checkTime);
            SetTransforms();
            barricade = Instantiate(barricade, transform.parent);
            CreateFireworks();
        }

        public void CollectBall() => collectedBallCount++;
        public void StartCollectingBalls()
        {
            StopCoroutine(checkSizeCoroutine);
            GameManager.Instance.StopMove();
            List<Ball> balls = ballManager.GetBalls(removeSize);
            foreach (var ball in balls)
            {
                ball.StartMoveToPool();
                ball.transform.parent = transform;
            }
            StartCoroutine(OnAllBallCollected(balls));
        }

        private bool CheckIsChild(Transform child)
        {
            return transform.GetComponentsInChildren<Transform>().Any(_transform => child == _transform);
        }

        private IEnumerator OnAllBallCollected(List<Ball> balls)
        {
            while (removeSize > collectedBallCount) yield return null;
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
            if(ballManager.TotalBallCount<=0)yield break;
            foreach (ParticleSystem firework in fireworks)
            {
                firework.Play();
                Destroy(firework.gameObject,2f);
            }
            GameManager.Instance.StartMove();
            ballManager.StartForwarding();
        }

        private void ReturnAllBallToPool(List<Ball> balls)
        {
            foreach (var ball in balls)
            {
                ball.RemoveBall();
                //ball.gameObject.SetActive(false);
                ball.transform.parent = BallPool.Instance.transform;
                ball.transform.localPosition = Vector3.zero;
                //BallManager.Instance.AddTotalBallCount(-1);
            }
        }
        private void CreateFireworks()
        {
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

        private void SetTransforms()
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
        }
        private IEnumerator CheckSize()
        {
            float newRemoveSize = 0;
            while (true)
            {
                newRemoveSize = ballManager.TotalBallCount * ((float)removePercentage / 100);
                removeSize = (int)Math.Round(newRemoveSize);
                if (removeSize <= 0) removeSize = 1;
                removeSizeText.text = removeSize.ToString();
                yield return wfsForCheckSize;
            }
        }

    }
}