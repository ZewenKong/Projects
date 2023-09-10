using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteControl : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Movement")]
    public float meteoriteSpeed = 1.5f;

    [Header("Explosion")]
    private int hitPoints = 6;
    public GameObject MeteoriteExplosionGO;

    [SerializeField] private Material flashMaterial;
    private Material originalMaterial;

    [Header("Score")]
    GameObject scoreUITextGO;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        flashMaterial = new Material(flashMaterial);
    }

    public void Start()
    {
        meteoriteSpeed = 1.5f;
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }

    public void Update()
    {
        Vector2 meteoritePosition = transform.position; // Get position
        meteoritePosition = new Vector2(meteoritePosition.x, meteoritePosition.y - meteoriteSpeed * Time.deltaTime); // Compute new position

        transform.position = meteoritePosition;

        Vector2 minLimit = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        if (transform.position.y < minLimit.y)
        {
            Destroy(this.gameObject);
        }
    }

    // - - - - - - - - - - Collision - - - - - - - - - -

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "PlayerBulletTag") || (collision.tag == "EnemyBulletTag"))
        {
            hitPoints--;
            StartCoroutine(Flash());

            if (hitPoints <= 0)
            {
                PlayMeteoriteExplosion();

                scoreUITextGO.GetComponent<GameScore>().Score += 3;

                Destroy(gameObject);
            }
        }
        else if (collision.tag == "PlayerShipTag")
        {
            PlayMeteoriteExplosion();

            scoreUITextGO.GetComponent<GameScore>().Score += 3;

            Destroy(gameObject);
        }
    }

    public IEnumerator Flash()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = originalMaterial;
    }

    // - - - - - - - - - - Explosion - - - - - - - - - -

    public void PlayMeteoriteExplosion()
    {
        GameObject meteoriteExplosion = (GameObject)Instantiate(MeteoriteExplosionGO);
        meteoriteExplosion.transform.position = transform.position;
    }
}
