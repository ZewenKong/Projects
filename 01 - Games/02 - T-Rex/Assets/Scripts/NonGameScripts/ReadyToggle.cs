using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class ReadyToggle : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public string isReady = "Not Ready";

    // Interaction with WebGL
    [DllImport("__Internal")]
    private static extern void SetReady(string isReady);

    public void toggleButton()
    {
        // https://www.youtube.com/watch?v=K2K6_uaJcLw

        if (buttonText.text == "Click to Ready")
        {
            buttonText.text = "Ready";
            isReady = "Ready";

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetReady(isReady);
#endif
        }
    }

    public void readyStateReceive(int readyState)
    {
        Debug.Log(readyState);
        Debug.Log(readyState.GetType());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        //if (readyState == 1)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //}
    }
}

