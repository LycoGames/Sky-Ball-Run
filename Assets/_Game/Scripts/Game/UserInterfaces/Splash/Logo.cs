using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Game.UserInterfaces.Splash
{
    public class Logo : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void PlayFadeOutAnimation(float duration)
        {
            image.DOColor(new Color(image.color.r, image.color.g, image.color.b, 0), duration).SetEase(Ease.InOutSine);
        }
    }
}
