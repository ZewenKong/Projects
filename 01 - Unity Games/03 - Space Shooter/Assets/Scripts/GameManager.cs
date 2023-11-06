using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject playerShip;
    public GameObject enemySpwaner;
    public GameObject gameOverGO;

    [Header("Player Name")]
    public Text playerNameUpdated;

    [Header("Score Text")]
    public GameObject playerScoreTitleGO;
    public GameObject scoreUITextGO;

    [Header("Holding Time")]
    private float spaceHoldTime = 0f;
    private float spaceHoldStartTime = 0f;

    public static GameManager Instance
    {
        get; private set;
    }

    public enum GameState
    {
        Playing,
        GameOver
    }

    GameState gameState;

    private void Start()
    {
        UpdatePlayerName();
        gameState = GameState.Playing;
    }

    public void Update()
    {
        TrackSpaceHoldTime();
    }

    [DllImport("__Internal")]
    private static extern void SetPlayerScore(int playerScore);

    [DllImport("__Internal")]
    private static extern void SetHoldingTime(float holdingTime);

    public void UpdateGameState()
    {
        switch (gameState)
        {
            case GameState.Playing:

                scoreUITextGO.GetComponent<GameScore>().Score = 0; // At start, set score to 0
                gameOverGO.SetActive(false);
                break;

            case GameState.GameOver:

                int finalScore = GetFinalScore();
                int playerScore = finalScore;

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetPlayerScore(playerScore);
#endif

                float holdingTime = spaceHoldTime;

                Debug.Log(holdingTime);

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetHoldingTime(holdingTime);
#endif

                enemySpwaner.GetComponent<EnemySpawner>().StopEnemySpawn(); // Stop enemy spawner
                enemySpwaner.GetComponent<EnemySpawner>().StopMeteoriteSpawn();

                gameOverGO.SetActive(true);
                playerScoreTitleGO.SetActive(false);
                scoreUITextGO.SetActive(false);

                break;
        }
    }

    public void SetGameState(GameState updatedGameState)
    {
        gameState = updatedGameState;
        UpdateGameState();
    }

    // - - - - - - - - - - Player Name - - - - - - - - - -

    public void UpdatePlayerName()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "DefaultName");
        playerNameUpdated.text = playerName;
    }

    // - - - - - - - - - - Player Final Score - - - - - - - - - -

    public int GetFinalScore()
    {
        GameScore gameScoreComponent = scoreUITextGO.GetComponent<GameScore>();
        return gameScoreComponent.Score;
    }

    // - - - - - - - - - - Data Collection - - - - - - - - - -

    private void TrackSpaceHoldTime()
    {
        if (Input.GetKeyDown("space"))
        {
            spaceHoldStartTime = Time.time;
        }
        else if (Input.GetKeyUp("space"))
        {
            spaceHoldTime += Time.time - spaceHoldStartTime;
        }
    }
}
