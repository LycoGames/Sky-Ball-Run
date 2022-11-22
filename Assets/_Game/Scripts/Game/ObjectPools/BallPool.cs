using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.ObjectPooling;
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
        protected override IEnumerator FillThePool()
        {
            pooledObjects = new List<GameObject>();
            GameObject ball;
            for (int i = 0; i < amountToPool; i++)
            {
                ball = Instantiate(objectToPool,transform);
                ball.gameObject.SetActive(false);
                pooledObjects.Add(ball.gameObject);
            }
            yield return null;
        }
    }
}
