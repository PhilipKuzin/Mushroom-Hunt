using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : SingletonGeneric<RoadGenerator>
{
    public GameObject roadTilePrefab;
    private List<GameObject> roads = new List<GameObject>();

    [SerializeField] private int maxRoadCount = 7;
    [SerializeField] private float maxSpeed = 10;

    public float speed = 0;

    private void Start()
    {
        PoolManager.Instance.Preload(roadTilePrefab, 7);  // предзагрузка участков дороги
        ResetLevel();
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
            PoolManager.Instance.Despawn(roads[0]);
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
        CollectController.Instance.ResetCollectController();
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
            pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 9);
        }

        int r = Random.Range (0, 2);

        if (r == 0)
        {
            GameObject go = PoolManager.Instance.Spawn(roadTilePrefab, pos, Quaternion.identity);
            go.transform.SetParent(transform);
            roads.Add(go);
        }
        else
        {
            GameObject go = PoolManager.Instance.Spawn(roadTilePrefab, pos, new Quaternion (0, 180 ,0,0));
            go.transform.SetParent(transform);
            roads.Add(go);
        }

    }
}

