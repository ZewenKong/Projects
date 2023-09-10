using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // - - - - - - - - - - Enemy Ship - - - - - - - - - -

    public GameObject EnemyGO;

    public float maxSpawnRateInSecond = 2.0f;
    public float spawnInNSeconds;

    // - - - - - - - - - - Meteorite - - - - - - - - - -

    public GameObject MeteoriteGO;

    public float maxMeteoriteSpawnRateInSecond = 10.0f;
    public float spawnMeteoriteInNSeconds;

    private void Start()
    {
        StartEnemySpawn();
        StartMeteoriteSpawn();
    }

    // - - - - - - - - - - Enemy Ship Spawn - - - - - - - - - -

    public void EnemySpawn()
    {
        Vector2 minLimit = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 maxLimit = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        GameObject enemy = Instantiate(EnemyGO);
        enemy.transform.position = new Vector2(Random.Range(minLimit.x, maxLimit.x), maxLimit.y);

        Debug.Log(Random.Range(minLimit.x, maxLimit.x));

        FurtherEnemySpawn();
    }

    public void FurtherEnemySpawn()
    {
        if (maxSpawnRateInSecond > 1.0f)
        {
            spawnInNSeconds = Random.Range(1f, maxSpawnRateInSecond);
        }
        else
        {
            spawnInNSeconds = 1.0f;
        }

        Invoke("EnemySpawn", spawnInNSeconds);
    }

    public void MoreEnemySpawn()
    {
        if (maxSpawnRateInSecond > 1.0f)
        {
            maxSpawnRateInSecond--;
        }
        if (maxSpawnRateInSecond == 1.0f)
        {
            CancelInvoke("MoreEnemySpawn");
        }
    }

    // - - - - - - - - - - Meteorite Spawn - - - - - - - - - -

    public void MeteoriteSpawn()
    {
        Vector2 minLimit = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 maxLimit = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        GameObject meteorite = Instantiate(MeteoriteGO);
        meteorite.transform.position = new Vector2(Random.Range(minLimit.x, maxLimit.x), maxLimit.y);

        FurtherMeteoriteSpawn();
    }

    public void FurtherMeteoriteSpawn()
    {
        if (maxMeteoriteSpawnRateInSecond > 1.0f)
        {
            spawnMeteoriteInNSeconds = Random.Range(1f, maxMeteoriteSpawnRateInSecond);
        }
        else
        {
            spawnMeteoriteInNSeconds = 1.0f;
        }

        Invoke("MeteoriteSpawn", spawnMeteoriteInNSeconds);
    }

    public void MoreMeteoriteSpawn()
    {
        if (maxMeteoriteSpawnRateInSecond > 1.0f)
        {
            maxMeteoriteSpawnRateInSecond--;
        }
        if (maxMeteoriteSpawnRateInSecond == 1.0f)
        {
            CancelInvoke("MoreMeteoriteSpawn");
        }
    }

    // - - - - - - - - - - Game Manager - - - - - - - - - -

    public void StartEnemySpawn()
    {
        maxSpawnRateInSecond = 2.0f;

        Invoke("EnemySpawn", maxSpawnRateInSecond);
        InvokeRepeating("MoreEnemySpawn", 0f, 10f);
    }

    public void StartMeteoriteSpawn()
    {
        maxMeteoriteSpawnRateInSecond = 10.0f;

        Invoke("MeteoriteSpawn", maxMeteoriteSpawnRateInSecond);
        InvokeRepeating("MoreMeteoriteSpawn", 0f, 50f);
    }

    public void StopEnemySpawn()
    {
        CancelInvoke("EnemySpawn");
        CancelInvoke("MoreEnemySpawn");
    }

    public void StopMeteoriteSpawn()
    {
        CancelInvoke("MeteoriteSpawn");
        CancelInvoke("MoreMeteoriteSpawn");
    }
}
