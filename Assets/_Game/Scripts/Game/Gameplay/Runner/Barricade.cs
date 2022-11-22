using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class Barricade : MonoBehaviour
    {
        [SerializeField] private List<Transform> barricades;
        [SerializeField] private float openTime=2f;
        public void OpenBarricades()
        {
            foreach (Transform barricade in barricades)
            {
                //barricade.DORotate(new Vector3(0,0,90),openTime);
            }
        }
    }
}
