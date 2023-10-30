using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [Header("Movement")] 
    public float enemySpeed = 2.5f;

    [Header("Explosion")]
    public GameObject ExplosionGO;

    [Header("Score")]
    GameObject scoreUITextGO;

    public void Start()
    {
        enemySpeed = 2.5f;
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag"); 
    }

    public void Update()
    {
        Vector2 enemyPosition = transform.position; // Get position
        enemyPosition = new Vector2(enemyPosition.x, enemyPosition.y - enemySpeed * Time.deltaTime); // Compute new position

        transform.position = enemyPosition; // Update position

        Vector2 minLimit = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        if (transform.position.y < minLimit.y)
        {
            Destroy(this.gameObject); // Leave the scree -> destory
        }
    }

    // - - - - - - - - - - Collision - - - - - - - - - -

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "PlayerShipTag") || (collision.tag == "PlayerBulletTag") ||
            (collision.tag == "MeteoriteTag"))
        {
            PlayExplosion();

            scoreUITextGO.GetComponent<GameScore>().Score += 10;

            Destroy(gameObject);
        }
    }

    // - - - - - - - - - - Explosion - - - - - - - - - -

    public void PlayExplosion()
    {
        GameObject enemyExplosion = (GameObject)Instantiate(ExplosionGO);
        enemyExplosion.transform.position = transform.position;
    }
}
