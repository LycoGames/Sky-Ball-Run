using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class BallAdderGate : Gate
    {
        [Range(0, 100)] [SerializeField] private int maxAddPercentage;
        [SerializeField] private TextMeshProUGUI ballCountText;
        private int addSize = 1;
        private BallManager ballManager;
        private int currentAddPercentage;

        

        private void OnEnable()
        {
            //OnEnterGate += AddBall;
            currentAddPercentage = UnityEngine.Random.Range(0, maxAddPercentage + 1);
            ballManager = BallManager.Instance;
            ballManager.OnTotalBallCountChange += CheckSize;
            CheckSize(0);
            Debug.Log("Gate Actived");
        }

        private void OnDisable()
        {
            //OnEnterGate -= AddBall;
            ballManager.OnTotalBallCountChange -= CheckSize;
            Debug.Log("Gate Disable");
        }

        private void AddBall()
        {
            BallManager.Instance.AddBall(addSize);
            gameObject.SetActive(false);
        }


        private void CheckSize(int x)
        {
            float newAddSize = ballManager.TotalBallCount * ((float)currentAddPercentage / 100);
            addSize = (int)Math.Round(newAddSize);
            if (addSize <= 0) addSize = 1;
            ballCountText.text = "+" + addSize;
        }
    }
}