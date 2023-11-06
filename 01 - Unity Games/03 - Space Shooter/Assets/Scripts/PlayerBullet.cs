using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float bulletSpeed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletSpeed = 8.0f;
    }
     
    // Update is called once per frame
    public void Update() 
    {
        Vector2 bulletPosition = transform.position;

        bulletPosition = new Vector2(bulletPosition.x, bulletPosition.y + bulletSpeed * Time.deltaTime);

        transform.position = bulletPosition;

        Vector2 maxBulletLimit = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); // Limit y-position

        if (transform.position. y > maxBulletLimit.y)
        {
            Destroy(gameObject);
        }
    }

    // - - - - - - - - - - Attack - - - - - - - - - -

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.tag == "EnemyShipTag") || (collision.tag == "EnemyBulletTag") ||
           (collision.tag == "MeteoriteTag"))
        {
            Destroy(gameObject);
        }
    }
}
