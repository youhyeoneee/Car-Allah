using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    public List<GameObject> roads;
    private float offset = 30f;
    void Start()
    {
        if (roads != null && roads.Count >= 0)
        {
            // Z 위치로 정렬
            roads = roads.OrderBy(r => r.transform.position.z).ToList();
        }
    }
    
    void Update()
    {
        
    }

    public void MoveRoad()
    {
        
        // 맨 앞 길 리스트에서 제거 
        GameObject moveRoad = roads[0];
        roads.Remove(moveRoad);
        
        // 맨 뒤 위치로 변경 후 리스트에 추가
        float newZ = roads[roads.Count - 1].transform.position.z + offset;
        moveRoad.transform.localPosition = new(0, 0, newZ);
        roads.Add(moveRoad);
        
        Debug.Log($"Spawn to {newZ}");

    }
}