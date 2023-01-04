using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Game.Gameplay.EndGames.Waterfall
{
    public class DiamondRewardVisualizer : MonoBehaviour
    {
        public Action<int> DiamondCollected;

        [SerializeField] private RectTransform diamondUiPrefab;
        [SerializeField] private float xRandomOffset = 50;

        private const float YTargetValue = 600f;
        private Vector2 diamondWealthPos;
        private Vector2 scaleDown = new(.6f, .6f);

        public Transform Parent { get; set; }

        private Camera mainCam;

        private void Start()
        {
            mainCam = Camera.main;
            diamondWealthPos = new Vector2(300, 680);
        }

        public void DiamondRewardSequence(Vector3 position, int gold)
        {
            var screenPos = mainCam.WorldToScreenPoint(position);
            var instance = Instantiate(diamondUiPrefab, screenPos, Quaternion.identity, Parent);
            var targetPos = screenPos;
            targetPos.y = YTargetValue;
            targetPos.x += GetRandomXOffset();
            var rewardSequence = DOTween.Sequence();
            var flyUpSequence = DOTween.Sequence();
            var punchSequence = DOTween.Sequence();
            punchSequence.Append(instance.DOScale(scaleDown, .25f));
            punchSequence.Append(instance.DOScale(Vector3.one, .25f)).SetLoops(-1);
            flyUpSequence.Append(instance.DOMove(targetPos, 1f)).OnComplete(() => punchSequence.Kill(true));
            rewardSequence.Append(flyUpSequence);
            rewardSequence.Append(instance.DOLocalMove(diamondWealthPos, .3f).SetEase(Ease.Linear));
            rewardSequence.OnComplete(() =>
            {
                DiamondCollected?.Invoke(gold);
                Destroy(instance.gameObject);
            });
        }

        private float GetRandomXOffset()
        {
            return Random.Range(-xRandomOffset, xRandomOffset);
        }
    }
}