using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotSpawner : MonoBehaviour
{
    private int initAmount = 5;
    private float plotSize = 30f;
    private float xPlotLeft = -36.3f;
    private float xPlotRight;
    private float lastZPos = 10f;

    public List<GameObject> plots;
    void Start()
    {
        xPlotRight = xPlotLeft * -1;

        for (int i = 0; i < initAmount; i++)
        {
            SpawnPlot();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlot()
    {
        GameObject plotLeft = plots[Random.Range(0, plots.Count)];            
        GameObject plotRight = plots[Random.Range(0, plots.Count)];

        float zPos = lastZPos + plotSize;
        lastZPos = zPos;

        Instantiate(plotLeft, new Vector3(xPlotLeft, 0.025f, zPos), plotLeft.transform.rotation);
        Instantiate(plotRight, new Vector3(xPlotRight, 0.025f, zPos), new Quaternion(0, 180, 0, 0));
    }
}
