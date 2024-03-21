using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject roadTilePrefab;
    private List<GameObject> roads = new List<GameObject>();

    public int maxRoadCount = 5;
    private float _speed = 0;
    public float maxSpeed = 10;
    
    private void Start()
    {
        ResetLevel();
        //StartLevel();
    }
    private void Update()
    {
        if (_speed == 0)
            return;

        foreach (var road in roads)
        {
            road.transform.position -= new Vector3( 0,0, _speed * Time.deltaTime);
        }
        if (roads[0].transform.position.z < -5)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);
            CreateNextRoadTile();
        }
    }

    public void ResetLevel()
    {
        _speed = 0;
        while (roads.Count > 0)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);
        }
        for (int i = 0; i < maxRoadCount; i++)
        {
            CreateNextRoadTile();
        }
    }

    public void StartLevel()
    {
        _speed = maxSpeed;
    }
    private void CreateNextRoadTile()
    {
        Vector3 pos = Vector3.zero;
        if (roads.Count > 0)
        {
            pos = roads[roads.Count - 1].transform.position + new Vector3(0,0,4);
        }
        GameObject go = Instantiate(roadTilePrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        roads.Add(go);
    }
}
