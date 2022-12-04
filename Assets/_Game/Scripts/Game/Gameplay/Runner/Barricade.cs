using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class Barricade : MonoBehaviour
    {
        [SerializeField] private List<Transform> barricades;
        [SerializeField] private float openTime=2f;

        void Start()
        {
            
        }
        public void OpenBarricades()
        {
            foreach (Transform barricade in barricades)
            {
                Debug.Log("Barricade Opening");
                StartCoroutine(RotateBarricade(barricade));
                //TODO AHMET BEY DOROTATE YAPACAK INSALLAH.
            }
        }

        private IEnumerator RotateBarricade(Transform barricade)
        {
            while (barricade.localRotation.z >= 89f)
            {
                barricade.localRotation=Quaternion.Euler(0,0,barricade.localRotation.z+openTime*Time.deltaTime);
                yield return null;
            }
        }
    }
}
