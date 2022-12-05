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
        [SerializeField] private CheckpointBarricade checkpointBarricade;
        [SerializeField] private Transform hood;
        [SerializeField] private int removePercentage=1;
        [SerializeField] private float coverCloseTime = 2f;
        [SerializeField] private TextMeshProUGUI removeSizeText;
        [SerializeField] private float checkTime=1f;
        [SerializeField] private List<ParticleSystem> fireworks;
        [SerializeField] private Transform dropPosition;
        [SerializeField] private int minTakeBallCount;
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
            removeSize = balls.Count;
            RemoveBalls(balls);
            foreach (var ball in balls)
            {
                ball.StartMoveToPool(dropPosition.position.z);
                ball.transform.parent = transform;
            }
            ReshapingBalls();
            StartCoroutine(OnAllBallCollected(balls));
        }
        

        private IEnumerator OnAllBallCollected(List<Ball> balls)
        {
            while (removeSize > collectedBallCount) yield return null;
            
            
            CloseHood();
            
            yield return new WaitForSeconds(coverCloseTime);
            ReturnAllBallToPool(balls);

            if (ballManager.TotalBallCount <= 0)
            {
                GameManager.Instance.OnRevive += OpenBarricades;
                yield break;
            }
            OpenBarricades();
            PlaySound();
            PlayFireWork();
            StartPlayMove();
        }

        private void PlaySound()
        {
            AudioSourceController.Instance.PlaySoundType(SoundType.PassCheckpoint);
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
            GameManager.Instance.OnRevive -= OpenBarricades;
            checkpointBarricade.OpenGates();
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
        private void RemoveBalls(List<Ball> balls)
        {
            foreach (var ball in balls)
            {
                ball.transform.parent = transform;
                ball.UnregisterBall();
            }
        }


        private IEnumerator CheckSize()
        {
            float newRemoveSize = 0;
            while (true)
            {
                newRemoveSize = ballManager.TotalBallCount * ((float)removePercentage / 100);
                removeSize = (int)Math.Round(newRemoveSize);
                if (removeSize <= minTakeBallCount) removeSize = minTakeBallCount;
                removeSizeText.text = removeSize.ToString();
                yield return wfsForCheckSize;
            }
        }

    }
}