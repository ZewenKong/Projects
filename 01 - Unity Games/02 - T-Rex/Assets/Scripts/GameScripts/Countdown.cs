using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Countdown : MonoBehaviour
{
    public float timeLeft; // Countdown Start
    public TextMeshProUGUI countdownText; // Text
    public AudioClip countdownSound; // Sound
    private bool isCountdown; // Start countdown or not
    private bool countdownFinished; // To track countdown finish

    public void Start()
    {
        isCountdown = true;
    }

    public void Update()
    {
        if (isCountdown)
        {
            timeLeft -= Time.deltaTime;
        }

        countdownText.text = Mathf.CeilToInt(timeLeft).ToString();
        
        if (timeLeft <= 0 && isCountdown)
        {
            isCountdown = false; // Stop counting
            this.GetComponent < AudioSource >().PlayOneShot(countdownSound); // Sound
            countdownFinished = true;
        }
        else if (countdownFinished)
        {
            countdownText.text = "Start!";
            StartCoroutine(StartSceneAfterDelay(0.5f));
        }
    }

    private IEnumerator StartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
