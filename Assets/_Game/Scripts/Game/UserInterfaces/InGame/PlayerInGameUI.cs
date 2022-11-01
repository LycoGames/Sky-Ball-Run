using System;
using TMPro;
using UnityEngine;

namespace Game.UserInterfaces.InGame
{
    public class PlayerInGameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpBallCount;
        private const int DefaultBallCount = 1;

        private void Start()
        {
            ResetBallCount();
        }

        public void SetBallCount(int ballCount)
        {
            tmpBallCount.text = ballCount.ToString();
        }

        private void ResetBallCount()
        {
            tmpBallCount.text = DefaultBallCount.ToString();
        }
    }
}