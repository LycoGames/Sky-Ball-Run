using _Game.Scripts.Base.UserInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Game.UserInterfaces.Main
{
    public class PrepareGameCanvas : BaseCanvas, IStartable
    {
        public delegate void PrepareGameRequestDelegate();

        public event PrepareGameRequestDelegate OnSettingsRequest;

        private const string DefaultCurrentLevelText = "1";
        private const string DefaultNextLevelText = "2";

        [SerializeField] private TMP_Text currentLevelText;
        [SerializeField] private TMP_Text nextLevelText;
        [SerializeField] private Slider levelProgressionSlider;

        public void OnStart()
        {
            ResetCurrentLevelText();
            ResetNextLevelText();
            ResetLevelProgressionSlider();
        }

        #region Changes

        public void SetCurrentLevelText(string value)
        {
            currentLevelText.text = value;
        }

        public void SetNextLevelText(string value)
        {
            nextLevelText.text = value;
        }

        public void SetLevelProgressionSliderValue(float value)
        {
            levelProgressionSlider.value = value;
        }

        private void ResetCurrentLevelText()
        {
            currentLevelText.text = DefaultCurrentLevelText;
        }

        private void ResetNextLevelText()
        {
            nextLevelText.text = DefaultNextLevelText;
        }

        private void ResetLevelProgressionSlider()
        {
            levelProgressionSlider.value = 0;
        }

        #endregion

        #region Requests

        public void RequestSettings()
        {
            OnSettingsRequest?.Invoke();
        }

        #endregion
    }
}