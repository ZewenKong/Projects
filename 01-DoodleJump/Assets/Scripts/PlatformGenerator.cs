using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject[] platformPrefebs;

    public float cameraHeight = 5.5f;  // 记录 camera 高度

    public Transform platformPool;  // 保留 platform pool

    public float minimumY = 1.0f;
    public float maximumY = 1.5f;

    float currentYPosition = 0f;

    void Start()
    {
        SpawnPlatformPool();
        while (currentYPosition < Camera.main.transform.position.y + cameraHeight)  // 最高只能生成到 camera position + camera height
        {
            PickPlatform();
        }
    }

    void SpawnPlatformPool()
    {
        int PlatformNumber = 150;
        int BrittlePlatformNumber = 20;

        for (int i = 0; i < PlatformNumber; i++)
        {
            GameObject platform = Instantiate(platformPrefebs[0], platformPool);  // 把普通板放入 Pool 内
            platform.SetActive(false);  // 隐藏
        }
        for (int i = 0; i < BrittlePlatformNumber; i++)
        {
            GameObject platform = Instantiate(platformPrefebs[1], platformPool);
            platform.SetActive(false);
        }
    }

    void PickPlatform()
    {
        currentYPosition += Random.Range(minimumY, maximumY);
        float XPosition = Random.Range(-3.5f, 3.5f);  // x 方向随机生成

        int p;

        do
        {
            p = Random.Range(0, platformPool.childCount);
        }
        while (platformPool.GetChild(p).gameObject.activeInHierarchy);  // 如果是画面内的 platform 的话就重新抽取

        platformPool.GetChild(p).position = new Vector2(XPosition, currentYPosition);  // 生成位置
        platformPool.GetChild(p).gameObject.SetActive(true);  // 显示 platform
    }

    void Update()
    {
        if (currentYPosition < Camera.main.transform.position.y + cameraHeight)
        {
            PickPlatform();
        }
    }
}

// - - - - - - - - - -

//public class LevelGenerator : MonoBehaviour
//{
//    public GameObject platformPrefeb;

//    public int numberofPlatforms;
//    public float levelWidth = 3f;
//    public float minimumY = .2f;
//    public float maximumY = 1.5f;

//    // Start is called before the first frame update
//    void Start()
//    {
//        Vector3 spawnPosition = new Vector3();

//        for (int i = 0; i < numberofPlatforms; i++)
//        {
//            // As eact instantiate, increase the spawn position in the y with random value
//            spawnPosition.y += Random.Range(minimumY, maximumY);
//            // the spawn position in the x with random value
//            spawnPosition.x = Random.Range(-levelWidth, levelWidth);

//            // Quaternion.identity -> won't rotate
//            Instantiate(platformPrefeb, spawnPosition, Quaternion.identity);
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
