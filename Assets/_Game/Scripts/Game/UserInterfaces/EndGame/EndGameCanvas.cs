using System;
using Base.UserInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UserInterfaces.EndGame
{
    public class EndGameCanvas : BaseCanvas, IStartable, IQuitable
    {
        public delegate void EndGameRequestDelegate();

        public event EndGameRequestDelegate OnReturnToMainRequest;


        [SerializeField] private TMP_Text completedLevelText;

        [SerializeField] private TMP_Text completedText;
        [SerializeField] private TMP_Text failedText;
        [SerializeField] private TMP_Text nextLevelText;
        [SerializeField] private TMP_Text retryLevelText;

        [SerializeField] private GameObject completedTextObject;
        [SerializeField] private GameObject failedTextObject;
        [SerializeField] private GameObject nextLevelTextObject;
        [SerializeField] private GameObject retryLevelTextObject;

        [SerializeField] private GameObject finalPhaseUI;

        [SerializeField] private GameObject levelSlider;
        [SerializeField] private GameObject score;

        [SerializeField] private GameObject endGameUI;

        [SerializeField] private TMP_Text currentLevelProgressionText;
        [SerializeField] private TMP_Text nextLevelProgressionText;
        [SerializeField] private Slider levelProgressionSlider;
        [SerializeField] private Slider cannonballCountSlider;
        [SerializeField] private TMP_Text cannonballCountText;

        public void OnStart()
        {
            DisableCompletedText();
            DisableFailedText();
            DisableNextLevelText();
            DisableRetryLevelText();
            DisableEndGameUI();
            DisableFinalPhaseUI();
            DisableCalculateScoreObjects();
        }


        public void OnQuit()
        {
        }

        #region Changes

        public void ChangeCannonballCount(string score, string targetScore)
        {
            cannonballCountText.text = score + "/" + targetScore;
            UpdateCannonballCountSlider(score, targetScore);
        }

        public void ChangeCurrentLevel(string value)
        {
            currentLevelProgressionText.text = value;
        }

        public void ChangeNextLevel(string value)
        {
            nextLevelProgressionText.text = value;
        }

        public void ChangeLevelProgressionSliderValue(float value)
        {
            levelProgressionSlider.value = value;
        }

        public void ChangeCompletedLevelText(string completedLevel)
        {
            completedLevelText.text = completedLevel;
        }

        public void ChangeLevelStatusText(string levelStatus)
        {
            completedText.text = levelStatus;
        }

        public void ChangeNextLevelText(string value)
        {
            nextLevelText.text = value;
        }

        public void EnableLevelResultObjects()
        {
            completedLevelText.enabled = true;
            completedText.enabled = true;
            nextLevelText.enabled = true;
        }

        public void EnableCalculateScoreObjects()
        {
            levelSlider.SetActive(true);
            score.SetActive(true);
        }

        public void EnableFinalPhaseObjects()
        {
            finalPhaseUI.SetActive(true);
            levelSlider.SetActive(true);
            score.SetActive(true);
        }

        public void EnableEndGameObjects()
        {
            finalPhaseUI.SetActive(false);
            endGameUI.SetActive(true);
        }

        public void EnableSuccessObjects()
        {
            completedTextObject.SetActive(true);
            nextLevelTextObject.SetActive(true);
            completedText.enabled = true;
            nextLevelText.enabled = true;
        }

        public void EnableFailObjects()
        {
            failedTextObject.SetActive(true);
            retryLevelTextObject.SetActive(true);
            failedText.enabled = true;
            retryLevelText.enabled = true;
        }

        #endregion

        public void RequestReturnToPreparingGame()
        {
            OnReturnToMainRequest?.Invoke();
        }

        private void UpdateCannonballCountSlider(string cannonballCount, string totalCannonballCount)
        {
            cannonballCountSlider.value = float.Parse(cannonballCount) / float.Parse(totalCannonballCount);
        }

        private void DisableCompletedText()
        {
            completedTextObject.SetActive(false);
            if (completedText.enabled)
                completedText.enabled = false;
        }

        private void DisableFailedText()
        {
            failedTextObject.SetActive(false);
            if (failedText.enabled)
                failedText.enabled = false;
        }

        private void DisableNextLevelText()
        {
            nextLevelTextObject.SetActive(false);
            if (nextLevelText.enabled)
                nextLevelText.enabled = false;
        }

        private void DisableRetryLevelText()
        {
            retryLevelTextObject.SetActive(false);
            if (retryLevelText.enabled)
                retryLevelText.enabled = false;
        }

        private void DisableCalculateScoreObjects()
        {
            levelSlider.SetActive(false);
            score.SetActive(false);
        }

        private void DisableFinalPhaseUI()
        {
            finalPhaseUI.SetActive(false);
        }


        private void DisableEndGameUI()
        {
            endGameUI.SetActive(false);
        }
    }
}