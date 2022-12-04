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
        [SerializeField] private Barricade barricade;
        [SerializeField] private Transform hood;
        [SerializeField] private int removePercentage=1;
        [SerializeField] private float coverCloseTime = 2f;
        [SerializeField] private TextMeshProUGUI removeSizeText;
        [SerializeField] private float checkTime=1f;
        [SerializeField] private List<ParticleSystem> fireworks;
        [SerializeField] private Transform dropPosition;
        private int removeSize;
        private int collectedBallCount;
        private BallManager ballManager;
        private WaitForSeconds wfsForCheckSize;
        private Coroutine checkSizeCoroutine;
        
        private void OnEnable()
        {
            ballManager=BallManager.Instance;
            wfsForCheckSize = new WaitForSeconds(checkTime);
            checkSizeCoroutine=StartCoroutine(CheckSize());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
        

        public void CollectBall() => collectedBallCount++;
        public void StartCollectingBalls()
        {
            StopCoroutine(checkSizeCoroutine);
            GameManager.Instance.StopMove();
            List<Ball> balls = ballManager.GetBalls(removeSize);
            foreach (var ball in balls)
            {
                ball.StartMoveToPool(dropPosition.position.z);
                ball.transform.parent = transform;
            }
            StartCoroutine(OnAllBallCollected(balls));
        }
        

        private IEnumerator OnAllBallCollected(List<Ball> balls)
        {
            while (removeSize > collectedBallCount) yield return null;
            
            OpenBarricades();
            CloseHood();
            
            yield return new WaitForSeconds(coverCloseTime);
            ReturnAllBallToPool(balls);
            
            if(ballManager.TotalBallCount<=0)yield break;
            PlayFireWork();
            StartPlayMove();
            ReshapingBalls();
        }

        private void ReshapingBalls()
        {
            ballManager.StartForwarding();
        }

        private static void StartPlayMove()
        {
            GameManager.Instance.StartMove();
        }

        private void CloseHood()
        {
            hood.transform.DOLocalMoveZ(0, coverCloseTime);
            Invoke("MoveDownHood",coverCloseTime);
        }

        private void MoveDownHood()
        {
            hood.transform.DOLocalMoveY(0, 0.5f);
        }

        private void OpenBarricades()
        {
            barricade.OpenBarricades();
        }

        private void PlayFireWork()
        {
            foreach (ParticleSystem firework in fireworks)
            {
                firework.Play();
            }
        }

        private void ReturnAllBallToPool(List<Ball> balls)
        {
            foreach (var ball in balls)
            {
                ball.RemoveBall();
                ball.transform.parent = BallPool.Instance.transform;
                ball.transform.localPosition = Vector3.zero;
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