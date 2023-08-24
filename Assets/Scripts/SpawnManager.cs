using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private RoadSpawner roadSpawner;
    
    void Start()
    {
        roadSpawner = GetComponent<RoadSpawner>();
    }

    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnTrigger"))
        {
            roadSpawner.MoveRoad();
        }

    }
}
