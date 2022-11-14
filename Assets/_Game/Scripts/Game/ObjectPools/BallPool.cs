using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.ObjectPooling;
using _Game.Scripts.Game.Gameplay.Runner.Ball;
using UnityEngine;

namespace _Game.Scripts.Game.ObjectPools
{
    public class BallPool : ObjectPool
    {
        public IEnumerator StartInstantiatePool()
        {
            FillThePool();
            yield return null;
        }
        protected override void FillThePool()
        {
            pooledObjects = new List<GameObject>();
            GameObject ball;
            for (int i = 0; i < amountToPool; i++)
            {
                ball = Instantiate(objectToPool,transform);
                ball.gameObject.SetActive(false);
                pooledObjects.Add(ball.gameObject);
            }
        }
    }
}
