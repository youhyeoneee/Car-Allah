using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public List<GameObject> obstacles;
    
    private int spawnInterval = 30;
    private int lastSpawnZ = 30;
    private int spawnAmount = 11;
    private int beforeObstacle = 0;
    private float xPos = -18.7f;
    void Start()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            SpawnObstacle();
        }
    }

    public void SpawnObstacle()
    {
        lastSpawnZ += spawnInterval;

        // 50%
        if (Random.Range(0, 3) == 0)
        {
            GameObject obstacle = obstacles[GenerateRandomNumber()]; // 이전 장애물 제외 하고 랜덤으로 생성
            Instantiate(obstacle, new Vector3(xPos, 0, lastSpawnZ), obstacle.transform.rotation);
        }
    }
    
    private int GenerateRandomNumber()
    {
        int randomValue = Random.Range(0, obstacles.Count);
        while (randomValue == beforeObstacle) // 이전 꺼와 같으면 
        {
            randomValue = Random.Range(0, obstacles.Count);
        }
        beforeObstacle = randomValue;
        return randomValue;
    }
}
