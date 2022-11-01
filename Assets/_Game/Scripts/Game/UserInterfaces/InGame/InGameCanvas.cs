using Base.UserInterface;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game.UserInterfaces.InGame
{
    public class InGameCanvas : BaseCanvas, IStartable
    {
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

        public void ChangeCurrentLevel(string value)
        {
            currentLevelText.text = value;
        }

        public void ChangeNextLevel(string value)
        {
            nextLevelText.text = value;
        }

        public void ChangeLevelProgressionSliderValue(float value)
        {
            levelProgressionSlider.value = value;
        }

        #endregion

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
    }
}