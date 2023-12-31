using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private RoadSpawner roadSpawner;
    private PlotSpawner plotSpawner;
    private ObstacleSpawner obstacleSpawner;

    
    void Start()
    {
        roadSpawner = GetComponent<RoadSpawner>();
        plotSpawner = GetComponent<PlotSpawner>();
        obstacleSpawner = GetComponent<ObstacleSpawner>();
    }

    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("SpawnTrigger"))
        {
            roadSpawner.MoveRoad();
            plotSpawner.SpawnPlot();
            obstacleSpawner.SpawnObstacle();
        }
    }
}