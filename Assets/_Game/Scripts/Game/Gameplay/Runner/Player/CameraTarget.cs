using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Player
{
    public class CameraTarget : MonoBehaviour
    {
        [Range(0, 100)] [SerializeField] private float xFollowPercentage;
        [SerializeField] private float yMoveDuration;
        private Transform playerControllerTransform;
        private Tween tweenYRef;
        private Vector3 newPos;
        private Vector3 target;
        public void InitiliazeCameraTarget(BallManager ballManager, Transform _playerControllerTransform)
        {
            playerControllerTransform = _playerControllerTransform;
            ballManager.ChangeCameraYPos += MoveNewYPos;
        }
        void Update()
        {
            if (playerControllerTransform == null) return;
            MoveToTarget();
        }

        private void MoveToTarget()
        {
            target = playerControllerTransform.position;
            newPos = transform.position;
            newPos.x = target.x * (xFollowPercentage / 100);
            newPos.z = target.z;
            transform.position = newPos;
        }

        void MoveNewYPos(float value)
        {
            if(tweenYRef!=null)tweenYRef.Kill();
            tweenYRef = transform.DOMoveY(value,yMoveDuration);
        }
    }
}
