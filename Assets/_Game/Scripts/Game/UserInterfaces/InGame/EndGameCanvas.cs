using System.Collections;
using _Game.Scripts.Base.UserInterface;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.UserInterfaces.InGame
{
    public class EndGameCanvas : BaseCanvas, IStartable, IQuitable
    {
        [SerializeField] private RectTransform coinSection;
        [SerializeField] private TMP_Text coinText;

        private readonly Vector3 coinChangeScale = new(.5f, .5f, .5f);
        private Tweener punchTweener;

        public void OnStart()
        {
        }

        public void OnQuit()
        {
        }

        public void ChangeCoin(string value)
        {
            coinText.text = value;
            if (punchTweener is { active: true })
            {
                punchTweener.Kill();
                coinSection.transform.localScale = Vector3.one;
            }

            punchTweener = coinSection.transform.DOPunchScale(coinChangeScale, .1f, 1, 0F);
        }
    }
}