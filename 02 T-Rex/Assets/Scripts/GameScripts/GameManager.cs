using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // public get variable, private set variable

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed {  get; private set; }

    private Player player;
    private Spawner spawner;

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiScoreText;
    public TextMeshProUGUI playerNameUpdated;
    
    public Button retryButton;

    private float score;

    private int spaceCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // this, this instance of the game mamager
        }
        else // Instance exist
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        UpdatePlayerName();

        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();

        NewGame();
    }

    // No longer use
    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        gameSpeed = initialGameSpeed;
        enabled = true;

        score = 0f; // Reset score

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);

        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        UpdateHiScore();
    }

    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;

        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceCount++;
        }
    }

    public void UpdatePlayerName()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "DefaultName"); // Use a default name if the player name is not set
        playerNameUpdated.text = playerName;
    }

    // No longer use
    public void UpdateHiScore()
    {
        float hiScore = PlayerPrefs.GetFloat("hiScore", 0);

        if (score > hiScore)
        {
            hiScore = score;
            PlayerPrefs.SetFloat("hiScore", hiScore);
        }

        hiScoreText.text = Mathf.FloorToInt(hiScore).ToString("D5");
    }

    // Interaction with WebGL
    [DllImport("__Internal")]
    private static extern void SetFinalScore(float fScore);

    public void UpdateFScore()
    {
        float fScore = PlayerPrefs.GetFloat("fScore", 0);

        fScore = score;
        PlayerPrefs.SetFloat("fScore", fScore);

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetFinalScore(fScore);
#endif
    }

    // Interaction with WebGL
    [DllImport("__Internal")]
    private static extern void SetSpaceClicked(int countValue);

    public void UpdateClickCount()
    {
        int countValue = spaceCount;

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetSpaceClicked(countValue);
#endif
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);

        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        UpdateHiScore();
        UpdateFScore();
        UpdateClickCount();
        Debug.Log("Space has been pressed " + spaceCount + " times.");
    }

    // Player respawn (in Player.cs)
    public void HandlePlayerCollide()
    {
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {
        // Make player disapper
        player.gameObject.SetActive(false); // Deactivate the player (when collide with obstacle)
        gameSpeed = gameSpeed - 0.5f; // Player speed decrease
        
        yield return new WaitForSeconds(0.5f); // Wait for 1 second to execute following code (to respawn)

        // Make player appear
        player.transform.position = player.spawnPosition; // Respawn position
        player.gameObject.SetActive(true); // Reactivate the player

        // Then, the player be invulnerable for 3 seconds
        player.isInvulnerable = true; // Set player to be invulnerable
        StartCoroutine(player.Flash());
        yield return new WaitForSeconds(3f); // Invulnerable for 3 seconds

        player.isInvulnerable = false; // Set player back to vulnerable after 3 seconds
        player.gameObject.SetActive(true); // Reset player
    }
}
