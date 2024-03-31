using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : SingletonGeneric<RoadGenerator>
{
    public GameObject roadTilePrefab;
    private List<GameObject> roads = new List<GameObject>();

    public int maxRoadCount = 5;
    public float speed = 0;  // заменено с _speed на public speed
    public float maxSpeed = 10;

    private void Start()
    {
        ResetLevel();
        //StartLevel();
    }
    private void Update()
    {
        if (speed == 0)
            return;

        foreach (var road in roads)
        {
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
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
        speed = 0;
        while (roads.Count > 0)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);
        }
        for (int i = 0; i < maxRoadCount; i++)
        {
            CreateNextRoadTile();
        }
        SwipeManager.Instance.enabled = false;
        MapGenerator.Instance.ResetMaps();
    }

    public void StartLevel()
    {
        speed = maxSpeed;
        SwipeManager.Instance.enabled = true;
    }
    private void CreateNextRoadTile()
    {
        Vector3 pos = Vector3.zero;
        if (roads.Count > 0)
        {
            pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 4);
        }
        int r = Random.Range (0, 2);
        if (r == 0)
        {
            GameObject go = Instantiate(roadTilePrefab, pos, Quaternion.identity);
            go.transform.SetParent(transform);
            roads.Add(go);
        }
        else
        {
            GameObject go = Instantiate(roadTilePrefab, pos, new Quaternion (0, 180 ,0,0));
            go.transform.SetParent(transform);
            roads.Add(go);
        }

    }
}
