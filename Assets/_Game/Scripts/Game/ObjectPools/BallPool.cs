using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.ObjectPooling;
using _Game.Scripts.Game.Gameplay.Runner;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using UnityEngine;

namespace _Game.Scripts.Game.ObjectPools
{
    public class BallPool : ObjectPool
    {
        public static BallPool Instance;

        public IEnumerator StartInstantiatePool(Material _ballMaterial,bool _isBonusLevel)
        {
            isBonusLevel = _isBonusLevel;
            Instance = this;
            ballMaterial = _ballMaterial;
            yield return StartCoroutine(FillThePool());
        }

        public List<Ball> GetAllActiveBall()
        {
            List<Ball> balls = new List<Ball>();
            for (int i = 0; i < amountToPool; i++)
            {
                if (pooledObjects[i].activeInHierarchy)
                {
                    balls.Add(pooledObjects[i].GetComponent<Ball>());
                }
            }

            return balls;
        }

        public void ReturnAllBallToPool()
        {
            List<Ball> activeBall = GetAllActiveBall();
            foreach (Ball ball in activeBall)
            {
                ball.ReturnToPool();
            }

            BallManager.Instance.ClearAllColumns();
        }

        protected override IEnumerator FillThePool()
        {
            
            for (int i = 0; i < amountToPool; i++)
            {
                InstantiateObject();
                if (i % 100 == 0) yield return null;
            }

            yield return null;
        }
    }
}