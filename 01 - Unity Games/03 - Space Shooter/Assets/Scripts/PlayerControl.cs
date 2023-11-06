using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Movement")]
    public float speed = 5.0f;

    [Header("Shooting")]
    public GameObject playerBulletGO;
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;

    public float shootRate = 0.15f;
    private float nextShoot = 0.0f;

    [Header("Explosion")]
    public GameObject ExplosionGO; // Explosion prefeb

    [Header("Player Live")]
    public Text PlayerLiveUIText;
    public int playerFullLive = 3; // Full live
    int playerLive; // Current live (Create here)
    float remainingChance;

    [Header("Respawn")]
    public bool isInvulnerable = false;

    // Flash materials
    [SerializeField] private Material flashMaterial;
    private Material originalMaterial;
    // Flash duration/times
    public float numberOfFlash;
    public float flashDuration;

    [Header("Game Manager")]
    public GameObject GameManagerGO;

    [DllImport("__Internal")]
    private static extern void SetRemainingChance(float remainingChance);

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        flashMaterial = new Material(flashMaterial);

        remainingChance = playerFullLive;

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetRemainingChance(remainingChance);
#endif
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        PlayerMovement();
        Shooting();
    }

    // - - - - - - - - - - Player Live - - - - - - - - - -

    public void Init()
    {
        playerLive = playerFullLive; // Current live
        PlayerLiveUIText.text = playerLive.ToString(); // Update to UI
        gameObject.SetActive(true);
    }

    // - - - - - - - - - - Player Movement - - - - - - - - - -

    public void PlayerMovement()
    {
        float x = Input.GetAxisRaw("Horizontal"); // -1 (left), 0 (no movement), 1 (right)
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(x, y).normalized; // Compute direction vector
        Move(direction);
    }

    public void Move(Vector2 direction)
    {
        Vector2 minLimit = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); // Bottom left point
        Vector2 maxLimit = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); // Top right

        maxLimit.x = maxLimit.x - 0.225f; // 0.225f (half player sprite width)
        minLimit.x = minLimit.x + 0.225f;

        maxLimit.y = maxLimit.y - 0.225f;
        minLimit.y = minLimit.y + 0.225f;

        Vector2 playerPosition = transform.position; // Player current position
        playerPosition += direction * speed * Time.deltaTime; // Player now position

        playerPosition.x = Mathf.Clamp(playerPosition.x, minLimit.x, maxLimit.x); // Set limit
        playerPosition.y = Mathf.Clamp(playerPosition.y, minLimit.y, maxLimit.y);

        transform.position = playerPosition;
    }

    // - - - - - - - - - - Shooting - - - - - - - - - -

    public void Shooting()
    {
        if (Input.GetKey("space") && Time.time > nextShoot)
        {
            GetComponent<AudioSource>().Play();

            nextShoot = Time.time + shootRate; // Set shoot interval

            GameObject bullet01 = (GameObject)Instantiate(playerBulletGO); // Instantiate the bullet
            bullet01.transform.position = bulletPosition01.transform.position; // Set initial position

            GameObject bullet02 = (GameObject)Instantiate(playerBulletGO);
            bullet02.transform.position = bulletPosition02.transform.position;
        }
    }

    // - - - - - - - - - - Collision - - - - - - - - - -

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "EnemyShipTag") || (collision.tag == "EnemyBulletTag") ||
            (collision.tag == "MeteoriteTag")) // Collide with enemy/enemy bullet
        {
            if (isInvulnerable)
            {
                return;
            }
            else
            {
                PlayExplosion();
                playerLive--;
                RemainingChance();
                PlayerLiveUIText.text = playerLive.ToString(); // Renew text

                if (playerLive == 0) // No more live
                {
                    GameManagerGO.GetComponent<GameManager>().SetGameState(GameManager.GameState.GameOver); // Set game over
                    gameObject.SetActive(false); // Hide player (Destroy(gameObject);)
                }
                else
                {
                    StartCoroutine(Respawn());
                }
            }
        }
    }

    // - - - - - - - - - - Explosion - - - - - - - - - -

    public void PlayExplosion()
    {
        GameObject playerExplosion = (GameObject)Instantiate(ExplosionGO);
        playerExplosion.transform.position = transform.position;
    }

    // - - - - - - - - - - Invulnerable - - - - - - - - - -

    public IEnumerator Respawn()
    {
        Vector2 currentPosition = transform.position;
        transform.position = currentPosition;
        gameObject.SetActive(true);  // Reactivate the player

        isInvulnerable = true;
        StartCoroutine(Flash());

        yield return new WaitForSeconds(3f);  // Invulnerable for 3 seconds

        isInvulnerable = false;
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

    // - - - - - - - - - - Player Live - - - - - - - - - -

    // Interaction with WebGL
    // [DllImport("__Internal")]
    // private static extern void SetRemainingChance(float remainingChance);

    public void RemainingChance()
    {
        if (playerLive == 3)
        {
            remainingChance = 3;
        }
        else if (playerLive == 2)
        {
            remainingChance = 2;
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetRemainingChance(remainingChance);
#endif
        }
        else if (playerLive == 1)
        {
            remainingChance = 1;
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetRemainingChance(remainingChance);
#endif
        } else
        {
            remainingChance = 0;
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetRemainingChance(remainingChance);
#endif
        }
    }
}
