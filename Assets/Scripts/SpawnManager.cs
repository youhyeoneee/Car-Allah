using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private RoadSpawner roadSpawner;
    private PlotSpawner plotSpawner;
    
    
    void Start()
    {
        roadSpawner = GetComponent<RoadSpawner>();
        plotSpawner = GetComponent<PlotSpawner>();
    }

    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("SpawnTrigger") && GameManager.Instance.gameState == GameState.Playing)
        {
            roadSpawner.MoveRoad();
            plotSpawner.SpawnPlot();
        }
    }
}