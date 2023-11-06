using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    Text scoreTextUI;
    int score = 0;

    public int Score
    {
        get { return this.score; } set { this.score = value; UpdateScore(); }
    }

    // Start is called before the first frame update
    private void Start()
    {
        scoreTextUI = GetComponent<Text>();
    }

    public void UpdateScore()
    {
        string scoreString = string.Format("{0:000000}", score);
        scoreTextUI.text = scoreString;
    }
}
