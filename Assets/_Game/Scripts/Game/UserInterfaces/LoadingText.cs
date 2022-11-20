using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    private string loadstr = "Loading";
    void Start()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        int i = 0;
        string addtext="";
        while (true)
        {
            for (int j = 0; j < i; j++) addtext += ".";
            loadingText.text = loadstr + addtext;
            addtext = "";
            i++;
            if (i > 3)i = 0; 
            yield return new WaitForSeconds(0.5f);
        }
    }
}
