using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.ObjectPooling;
using _Game.Scripts.Game.Gameplay.Runner;
using UnityEngine;

namespace _Game.Scripts.Game.ObjectPools
{
    public class BallPool : ObjectPool
    {
        public static BallPool Instance;

    

        public IEnumerator StartInstantiatePool()
        {
            Instance = this;
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
        protected override IEnumerator FillThePool()
        {
            pooledObjects = new List<GameObject>();
            GameObject ball;
            for (int i = 0; i < amountToPool; i++)
            {
                ball = Instantiate(objectToPool,transform);
                ball.gameObject.SetActive(false);
                pooledObjects.Add(ball.gameObject);
                if (i % 20 == 0) yield return null;
            }
            yield return null;
        }
    }
}
