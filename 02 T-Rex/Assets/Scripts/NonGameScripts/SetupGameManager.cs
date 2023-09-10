using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class SetupGameManager : MonoBehaviour
{
    public TMP_InputField playerName;
    public Button createButton;
    
    public string isReady = "";

    public float noResponseTime = 30.0f; // Timer, when not enter

    public void Start()
    {
        PlayerPrefs.DeleteKey("PlayerName");
        playerName.text = "";

        // To G.M.
        string playerNameUpdated = PlayerPrefs.GetString("PlayerName", "");
        playerName.text = playerNameUpdated;

        // Button (Scence change)
        createButton.onClick.AddListener(LoadScene);
    }

    public void Update()
    {
        noResponseTime -= Time.deltaTime;

        if (noResponseTime <= 0.0f)
        {
            NoResponse();
        }
    }

    // In JSlib
    // https://github.com/jeffreylanters/react-unity-webgl/issues/49

    // Interaction with WebGL
    [DllImport("__Internal")]
    private static extern void SetPlayerName(string playerNameSet);

    // Interaction with WebGL
    [DllImport("__Internal")]
    private static extern void SetReady(string isReady);

    public void SetPlayerName()
    {
        // To G.M.
        string playerNameUpdated = playerName.text;
        PlayerPrefs.SetString("PlayerName", playerNameUpdated);

        // To WebGL
        string playerNameSet = playerName.text;
        isReady = "Not Ready";

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetPlayerName(playerNameSet);
            SetReady(isReady);
#endif
    }

    public void LoadScene()
    {
        SetPlayerName();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NoResponse()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }
}
