using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Base
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fpsText;
        [SerializeField] float refreshTime = 1f;
        private float fps;
        private WaitForSeconds wfsForRefreshFPS;
        void Start()
        {
            Application.targetFrameRate = 60;
            wfsForRefreshFPS = new WaitForSeconds(refreshTime);
            StartCoroutine(GetFps());
        }

        IEnumerator GetFps()
        {
            while (true)
            {
                fps = (int)(1f / Time.unscaledDeltaTime);
                fpsText.text = fps.ToString();
                yield return wfsForRefreshFPS;
            }
        }
    }
}
