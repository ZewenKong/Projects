using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    public TMP_Text textComponent;
    public string fullText;
    public float delayBetweenCharacters = 0.1f; // Time in seconds between each character appearing
    public float delayBeforeRestart = 1f; // Time in seconds before the animation restarts
    public float delayBeforeClearing = 1f; // Time in seconds before clearing the text

    void Start()
    {
        // Start the coroutine that will handle the animation
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        // Infinite loop to repeatedly play the animation
        while (true)
        {
            // Loop through each character in the fullText string
            for (int i = 0; i < fullText.Length; i++)
            {
                // Set the text to the substring up to the current character
                textComponent.text = fullText.Substring(0, i + 1);

                // Wait for the specified delay before showing the next character
                yield return new WaitForSeconds(delayBetweenCharacters);
            }

            // Wait for the specified delay before clearing the text
            yield return new WaitForSeconds(delayBeforeClearing);

            // Clear the text
            textComponent.text = "";

            // Wait for the specified delay before restarting the animation
            yield return new WaitForSeconds(delayBeforeRestart);
        }
    }
}
