using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFieldController : Singleton<TimeFieldController>
{
    Grid coordGrid;

    float[,] data;
    int dataSize = 60;
    int halfDataSize = 30;

    [NonSerialized] public List<Vector3> TargetList;


    void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        StopCoroutine(PopulateTargetListRoutine());
        coordGrid = GetComponent<Grid>();
        data = new float[dataSize, dataSize];
        TargetList = new List<Vector3>();
        StartCoroutine(PopulateTargetListRoutine());
    }


    void AddRandomTarget()
    {
        int x = UnityEngine.Random.Range(-halfDataSize, halfDataSize);
        int y = UnityEngine.Random.Range(-halfDataSize, halfDataSize);

        data[x + halfDataSize, y + halfDataSize] = 0.1f;
        TargetList.Add(coordGrid.CellToWorld(new Vector3Int(x, y)));
    }

    void PopulateTargetList()
    {
        TargetList = new List<Vector3>();
        for (int x = 0; x < dataSize; x++)
            for (int y = 0; y < dataSize; y++)
            {
                if (data[x, y] > 0)
                    TargetList.Add(coordGrid.CellToWorld(new Vector3Int(x - halfDataSize, y - halfDataSize)));

            }

        while (TargetList.Count < 10)
        {
            AddRandomTarget();
        }

        // TargetList.Sort();
        Debug.Log($"targetList sorted with {TargetList.Count} entries:");
        // foreach (var item in TargetList)
        // {
        //     Debug.Log(item);

        // }
    }

    public Vector3 GetRandomWorldTarget()
    {
        int index = UnityEngine.Random.Range(0, TargetList.Count);
        return TargetList[index];
        //TODO add weighted
    }

    IEnumerator PopulateTargetListRoutine()
    {
        while (true)
        {
            PopulateTargetList();
            yield return new WaitForSecondsRealtime(2.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
