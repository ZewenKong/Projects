using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector2 _direction;
    public float enemyBulletSpeed;
    public bool isReady;

    private void Awake()
    {
        enemyBulletSpeed = 5.0f;
        isReady = false;
    }

    private void Update()
    {
        if (isReady)
        {
            Vector2 enemyBulletPosition = transform.position; // Get enemy bullet position
            enemyBulletPosition += _direction * enemyBulletSpeed * Time.deltaTime; // Compute
            transform.position = enemyBulletPosition; // Update

            Vector2 minLimit = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 maxLimit = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            if ((transform.position.x < minLimit.x) || (transform.position.x > maxLimit.x) ||
                (transform.position.y < minLimit.y) || (transform.position.y > maxLimit.y))
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized; 
        isReady = true;
    }

    // - - - - - - - - - - Attack - - - - - - - - - -

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "PlayerShipTag") || (collision.tag == "MeteoriteShipTag")) 
        {
            Destroy(gameObject);
        }
    }
}
