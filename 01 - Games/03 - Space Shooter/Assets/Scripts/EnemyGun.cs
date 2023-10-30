using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject EnemyBulletGO;

    public void Start()
    {
        Invoke("EnemyBulletShoot", 1f);
    }

    public void EnemyBulletShoot()
    {
        GameObject playerShip = GameObject.Find("PlayerGO");

        if (playerShip != null) // Player live
        {
            GameObject enemyBullet = (GameObject)Instantiate(EnemyBulletGO); // Instantiate the bullet

            enemyBullet.transform.position = transform.position; // Set position
            Vector2 bulletDirection = playerShip.transform.position - enemyBullet.transform.position; // Compute
            enemyBullet.GetComponent<EnemyBullet>().SetDirection(bulletDirection);
        }
    }
}
