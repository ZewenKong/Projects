using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController character;
    private SpriteRenderer spriteRenderer;
    private Vector3 direction;

    [Header("Jump")]
    public float gravity = 9.81f * 2.5f;
    public float jumpForce = 8f;

    [Header("Heart")]
    public GameObject heart01, heart02, heart03;
    private int heart = 3;
    public float heartNumber;

    [Header("Respawn Position")]
    public Vector3 initialPosition;
    public Vector3 positionOffset = new Vector3(0, 0, 0);
    public Vector3 spawnPosition;

    [Header("Invulnerability Frames")]
    public bool isInvulnerable = false;

    [Header("Flash Effect")]
    // Flash materials
    [SerializeField] private Material flashMaterial;
    private Material originalMaterial;
    // Flash duration/times
    public float numberOfFlash;
    public float flashDuration;

    // Interaction with WebGL
    [DllImport("__Internal")]
    private static extern void SetHeartNumber(float heartNumber);

    private void Awake() // Unity calls automatically when the script initialised
    {
        character = GetComponent<CharacterController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalMaterial = spriteRenderer.material;
        flashMaterial = new Material(flashMaterial);

        initialPosition = transform.position; // Get the initial position (use for respawn)

        heartNumber = 3; // The original heart number (3) // Send to jslib

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetHeartNumber(heartNumber);
#endif
    }

    private void OnEnable()
    {
        direction = Vector3.zero; // When re-enable the player, reset the direction to zero
    }

    private void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            direction = Vector3.down;

            if (Input.GetButton("Jump")) // GetButton(), can hold the button to execute
            {
                direction = Vector3.up * jumpForce;
            }
        }
        character.Move(direction * Time.deltaTime); // Apply to character
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvulnerable) // If invulnerable, destory the obstacle
        {
            Destroy(other.gameObject);
            return;
        }

        if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject); // When collide the obstacle, destory it
            spawnPosition = transform.position + positionOffset; // Respawn position (-6, 0, 0)

            heart--; // Heart decreases
            Heart();
        }
    }

    public IEnumerator Flash()
    {
        int numberOfFlash = 6;
        float singleFlashDuration = 3f / (2 * numberOfFlash);

        for (int i = 0; i < numberOfFlash; i++)
        {
            spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(singleFlashDuration);
            spriteRenderer.material = originalMaterial;
            yield return new WaitForSeconds(singleFlashDuration);
        }
    }

    private void Heart()
    {
        if (heart == 3)
        {
            heart03.SetActive(true);
            heart02.SetActive(true);
            heart01.SetActive(true);

            GameManager.Instance.HandlePlayerCollide();
        }
        else if (heart == 2)
        {
            heart03.SetActive(false);
            heart02.SetActive(true);
            heart01.SetActive(true);

            heartNumber = 2;

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetHeartNumber(heartNumber);
#endif

            GameManager.Instance.HandlePlayerCollide();
        }
        else if (heart == 1)
        {
            heart03.SetActive(false);
            heart02.SetActive(false);
            heart01.SetActive(true);

            heartNumber = 1;

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetHeartNumber(heartNumber);
#endif

            GameManager.Instance.HandlePlayerCollide();
        }
        else if (heart < 1)
        {
            heart03.SetActive(false);
            heart02.SetActive(false);
            heart01.SetActive(false);

            heartNumber = 0;

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetHeartNumber(heartNumber);
#endif

            GameManager.Instance.GameOver();
        }
    }
}
