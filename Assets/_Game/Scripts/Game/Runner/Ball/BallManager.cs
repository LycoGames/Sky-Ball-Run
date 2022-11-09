using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts.Game.Runner.Ball
{
    public class BallManager : MonoBehaviour
    {
        private Trail trail;
        private BallManager nextBall;
        private Action onRemove;
        [SerializeField] private BallMover ballMover;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private ParticleSystem animation;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                meshRenderer.enabled = false;
                animation.Play();
                trail.RemoveBallInQueue(this);
            }
        }

        public void RemoveBall()
        {
            UnRegister();
            onRemove?.Invoke();
            Destroy();
        }
        
        public void InitializeBall(Trail _trail)
        {
            trail = _trail;
            SetNextBall();
            
        }
        public void SetNextBall()
        {
            nextBall = trail.GetNextBall(this);
            Register();
            SetFollow();
            ballMover.SetOffset();
        }

        private void SetFollow()
        {
            Transform follow;
            if (nextBall != null) follow = nextBall.transform;
            else follow = trail.transform;
            ballMover.SetFollow(follow);
        }

        private void Register()
        {
            if (nextBall != null) nextBall.onRemove += SetNextBall;
        }

        private void UnRegister()
        {
            if (nextBall != null) nextBall.onRemove -= SetNextBall;
        }
        private void Destroy()
        {
            gameObject.SetActive(false);
            //TODO go back to pool
        }
    }
}
