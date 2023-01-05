using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Tutorials
{
    public class TutorialCanvas : MonoBehaviour
    {
        [SerializeField] private float waitForRemove = 1f;
        [SerializeField] private float autoKillTime = 5f;
        private bool isFirstTouch = true;

        private void Start()
        {
            if(autoKillTime>0)Invoke("DestroyCanvas",autoKillTime);
        }

        private void Update()
        {
            if (isFirstTouch&&Input.GetMouseButtonDown(0))
            {
                isFirstTouch = false;
                Invoke("DestroyCanvas",waitForRemove);
            }
        }

        private void DestroyCanvas()
        {
            Destroy(gameObject);
        }
    }
}
