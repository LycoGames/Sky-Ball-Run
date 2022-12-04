using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Game.UserInterfaces.Loading
{
    public class LoadingImage : MonoBehaviour
    {
        [SerializeField] private float turningSpeed=2f;
        private readonly Vector3 _rotationVector = new Vector3(0, 0, -30);

        public void PlayLoadingTextAnimation()
        {
            StartCoroutine(Loading());
        }

        public void StopLoadingTextAnimation()
        {
            StopAllCoroutines();
        }

        IEnumerator Loading()
        {
            while (true)
            {
                transform.Rotate(Time.deltaTime * _rotationVector*turningSpeed);
                yield return null;
            }
        }
    }
}
