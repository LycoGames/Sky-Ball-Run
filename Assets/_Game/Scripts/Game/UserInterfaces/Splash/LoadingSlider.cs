using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UserInterfaces.Splash
{
    public class LoadingSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        public void PlaySlider(float time)
        {
            slider.DOValue(1, time);
        }
    }
}