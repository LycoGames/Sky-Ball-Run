using _Game.Scripts.Base.UserInterface;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.UserInterfaces.InGame
{
    public class EndGameCanvas : BaseCanvas, IStartable, IQuitable
    {
        [SerializeField] private TMP_Text coinText;

        public void OnStart()
        {
        }

        public void OnQuit()
        {
        }

        public void ChangeCoin(string value)
        {
            coinText.text = value;
            coinText.transform.DOScale(Vector3.one, .2f).SetEase(Ease.OutQuad);
        }
    }
}