using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class StartPageGameManager : MonoBehaviour
{
    [Header("Input Field")]
    public InputField playerName;

    [Header("Button")]
    public Button setUpBtn;
    public Button exitBtn;

    [Header("Ready State")]
    public string readyState;

    [Header("No Response")]
    public float noResponseTime = 30.0f;

    [Header("Text Animation")]
    public Text loadingText;
    public string fullText;
    public float delayBetweenCharacters = 0.1f;
    public float delayBeforeRestart = 1f;
    public float delayBeforeClearing = 1f;

    public void Start()
    {
        // - - - - - - - - - - To GameManager.cs - - - - - - - - - -

        PlayerPrefs.DeleteKey("PlayerName");
        playerName.text = "";
        string playerNameUpdated = PlayerPrefs.GetString("PlayerName", "");
        playerName.text = playerNameUpdated;

        StartCoroutine(AnimateText());

        playerName.onValueChanged.AddListener(delegate { ValidateInput(); });
        ValidateInput();

        setUpBtn.onClick.AddListener(SetUp); // Set up the game
    }

    public void Update()
    {
        noResponseTime -= Time.deltaTime;

        if (noResponseTime <= 0.0f)
        {
            DonotPlay(); // If no more action, jump to exit page
        }
    }

    public void ValidateInput()
    {
        if (string.IsNullOrEmpty(playerName.text))
        {
            setUpBtn.interactable = false; // Disable the setUpBtn
        }
        else
        {
            setUpBtn.interactable = true; // Enable the setUpBtn
        }
    }

    // Interaction with WebGL
    [DllImport("__Internal")]
    private static extern void SetPlayerName(string playerNameSet);

    [DllImport("__Internal")]
    private static extern void SetReadyState(string readyState);

    public void PlayerNameSetUp()
    {
        // - - - - - - - - - - To GameManager.cs - - - - - - - - - -

        string playerNameUpdated = playerName.text;
        PlayerPrefs.SetString("PlayerName", playerNameUpdated);

        // - - - - - - - - - - To WebGL - - - - - - - - - -

        string playerNameSet = playerName.text;
        readyState = "Not Ready";

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetPlayerName(playerNameSet);
            SetReadyState(readyState);
#endif

    }

    public void SetUp()
    {
        PlayerNameSetUp();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DonotPlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
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
