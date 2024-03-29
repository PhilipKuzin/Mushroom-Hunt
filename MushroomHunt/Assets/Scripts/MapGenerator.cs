using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : SingletonGeneric<MapGenerator>
{
    [SerializeField] private GameObject _stumpPrefab;
    [SerializeField] private GameObject _treePrefab;
    [SerializeField] private GameObject _mushroomPrefab;

    private int _itemSpace = 15;
    private int _itemCountInMap = 5;
    private float _LaneOffset = 2f;           // в playercontroller брать lane offset отсюда! 

    private int _mushroomCountInItem = 10;
    private float _mushroomHeight = 0.25f;

    enum TrackPos { Left = -1, Center = 0, Right = 1 };
    enum MushroomStyle { Line, Jump };

    public List<GameObject> maps = new List<GameObject>();
    public List<GameObject> activeMaps = new List<GameObject>();

    private void Start()
    {
        maps.Add(MakeMap1());
        maps.Add(MakeMap1());
        maps.Add(MakeMap1());

        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }
    }

    private void Update()
    {
        if (RoadGenerator.Instance.speed == 0) { return; }

        foreach (GameObject map in activeMaps)
        {
            map.transform.position -= new Vector3(0, 0, RoadGenerator.Instance.speed * Time.deltaTime);
        }
        if (activeMaps[0].transform.position.z < -_itemCountInMap * _itemSpace)
        {
            RemoveFirstActiveMap(); 
            AddActiveMap();
        }
    }

    private void AddActiveMap()
    {
        int r = UnityEngine.Random.Range(0, maps.Count);
        GameObject go = maps[r];
        go.SetActive(true);
        foreach (Transform child in go.transform)
        {
            child.gameObject.SetActive(true);
        }
        go.transform.position = activeMaps.Count > 0 ?
            activeMaps[activeMaps.Count - 1].transform.position + Vector3.forward * _itemCountInMap * _itemSpace :
            new Vector3(0, 0, 10);

        maps.RemoveAt(r);
        activeMaps.Add(go);
    }

    private void RemoveFirstActiveMap()
    {
        activeMaps[0].SetActive(false);
        maps.Add(activeMaps[0]);
        activeMaps.RemoveAt(0);
    }

    public void ResetMaps()
    {
        while (activeMaps.Count > 0)
        {
            RemoveFirstActiveMap();
        }
        AddActiveMap();
        AddActiveMap();
    }

    private GameObject MakeMap1()
    {
        GameObject result = new GameObject();
        result.transform.SetParent(transform);  // чтобы на сцене result закидывался в иерархии к map generator 

        for (int i = 0; i < _itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            MushroomStyle mushroomStyle = MushroomStyle.Line;

            if (i == 2) { trackPos = TrackPos.Left; obstacle = _stumpPrefab; mushroomStyle = MushroomStyle.Jump; }
            else if (i == 3) { trackPos = TrackPos.Center; obstacle = _stumpPrefab; mushroomStyle = MushroomStyle.Jump; }
            else if (i == 4) { trackPos = TrackPos.Center; obstacle = _stumpPrefab; mushroomStyle = MushroomStyle.Jump; }

            Vector3 obstaclePos = new Vector3((int)trackPos * _LaneOffset, 0, i * _itemSpace);

            CreateMushrooms(mushroomStyle, obstaclePos, result);

            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }

    private void CreateMushrooms(MushroomStyle style, Vector3 pos, GameObject parentObject)
    {
        Vector3 mushroomPos = Vector3.zero;

        if (style == MushroomStyle.Line)
        {
            for (int i = -_mushroomCountInItem / 2; i < _mushroomCountInItem / 2; i++)
            {
                mushroomPos.y = _mushroomHeight;
                mushroomPos.z = i * ((float)_itemSpace / _mushroomCountInItem);
                GameObject go = Instantiate(_mushroomPrefab, mushroomPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }

        }
        else if (style == MushroomStyle.Jump)
        {
            for (int i = -_mushroomCountInItem / 2; i < _mushroomCountInItem / 2; i++)
            {
                mushroomPos.y = Mathf.Max(-1 / 2f * Mathf.Pow(i, 2) + 2, _mushroomHeight);
                mushroomPos.z = i * ((float)_itemSpace / _mushroomCountInItem);
                GameObject go = Instantiate(_mushroomPrefab, mushroomPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
    }

}
