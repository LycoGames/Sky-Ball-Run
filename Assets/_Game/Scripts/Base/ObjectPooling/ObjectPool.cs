using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Base.ObjectPooling
{
    public abstract class ObjectPool : MonoBehaviour
    {
        public List<GameObject> pooledObjects;
        public GameObject objectToPool;
        public int amountToPool;


        protected abstract IEnumerator FillThePool();

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }

            return null;
        }
    }
}