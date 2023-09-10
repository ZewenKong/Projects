using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;

public class LoadingPageGameManager : MonoBehaviour
{
    public Text buttonText;
    public Text loadingText;
    public string readyState = "Not Ready";

    public string fullText;
    public float delayBetweenCharacters = 0.1f;
    public float delayBeforeRestart = 1f;
    public float delayBeforeClearing = 1f;

    public void Start()
    {
        StartCoroutine(AnimateText());
    }

    // Interaction with WebGL
    [DllImport("__Internal")]
    private static extern void SetReadyState(string readyState);

    public void TextChange()
    {
        if (buttonText.text == "Click to Ready")
        {
            buttonText.text = "Ready";
            readyState = "Ready";
            Debug.Log(readyState);

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetReadyState(readyState);
#endif
        }
    }

    public void readyStateReceive(int readyState)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // - - - - - - - - - - Text Animation - - - - - - - - - -

    IEnumerator AnimateText()
    {
        while (true)
        {
            for (int i = 0; i < fullText.Length; i++)
            {
                loadingText.text = fullText.Substring(0, i + 1);

                yield return new WaitForSeconds(delayBetweenCharacters);
            }

            yield return new WaitForSeconds(delayBeforeClearing);

            loadingText.text = "";

            yield return new WaitForSeconds(delayBeforeRestart);
        }
    }
}
