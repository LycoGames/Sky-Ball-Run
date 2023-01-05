using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.ObjectPooling;
using _Game.Scripts.Game.Gameplay.Runner;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Game.ObjectPools
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance;
        public List<GameObject> pooledObjects=new List<GameObject>();
        public GameObject objectToPool;
        public int amountToPool;

        private int objectToPoolCount;
        

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }

            amountToPool++;
            return InstantiateObject();
        }

        public GameObject InstantiateObject()
        {
            GameObject ball;
            ball = Instantiate(objectToPool, transform);
            ball.gameObject.SetActive(false);
            pooledObjects.Add(ball.gameObject);
            return ball;
        }
        

        public IEnumerator StartInstantiatePool(GameObject _objectToPool)
        {
            objectToPool = _objectToPool;
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

        public void ReturnAllBallToPool()
        {
            List<Ball> activeBall = GetAllActiveBall();
            foreach (Ball ball in activeBall)
            {
                ball.ReturnToPool();
            }

            BallManager.Instance.ClearAllColumns();
        }

        protected IEnumerator FillThePool()
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