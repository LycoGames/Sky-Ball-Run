using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Gameplay.EndGames.Waterfall;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class EndGameComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
        public delegate void EndGameChangeDelegate();

        public event EndGameChangeDelegate OnSuccess;
        public event EndGameChangeDelegate OnFail;

        [SerializeField] private List<WaterfallBasket> waterfallBasketList;

        public int EndGamePoints { get; set; }

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
        }

        public void OnConstruct()
        {
            StartCoroutine(WaterfallEndGameCoroutine());
        }

        public void OnDestruct()
        {
            UnRegisterActions();
        }


        private IEnumerator WaterfallEndGameCoroutine()
        {
            SetupBasketList();
            yield return BallManager.Instance.GetWaterfallForm();
            yield return BallManager.Instance.MoveDownwards();
        }

        private void IncreasePoint(int points)
        {
            EndGamePoints += points;
            Debug.LogError(EndGamePoints);
        }

        private void SetupBasketList()
        {
            foreach (var basket in waterfallBasketList)
            {
                basket.GoldCollected += IncreasePoint;
            }
        }


        private void UnRegisterActions()
        {
            foreach (var basket in waterfallBasketList)
            {
                basket.GoldCollected -= IncreasePoint;
            }
        }
    }
}