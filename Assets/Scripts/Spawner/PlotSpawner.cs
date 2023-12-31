using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotSpawner : MonoBehaviour
{
    private int initAmount = 11;
    private float plotSize = 30f;
    private float xPlotLeft = -73.67f;
    private float xPlotRight = 36.3f;
    private float lastZPos = 10f;
    private float yPos = 0.025f;

    public List<GameObject> plots;
    public GameObject emptyPlot;
    private float offset = 30f;

    void Start()
    {
        for (int i = 0; i < initAmount; i++)
        {
            SpawnPlot();
        }
    }
    
    public void SpawnPlot()
    {
        GameObject plotLeft = plots[Random.Range(0, plots.Count)];            

        float zPos = lastZPos + plotSize;       
        lastZPos = zPos;

        Instantiate(plotLeft, new Vector3(xPlotLeft, yPos, zPos), plotLeft.transform.rotation);
        Instantiate(emptyPlot, new Vector3(xPlotRight, yPos, zPos), new Quaternion(0, 180, 0, 0));
    }
}
