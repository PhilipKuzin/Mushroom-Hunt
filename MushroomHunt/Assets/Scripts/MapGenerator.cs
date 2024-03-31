using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapGenerator : SingletonGeneric<MapGenerator>
{
    [SerializeField] private GameObject _stumpPrefab;
    [SerializeField] private GameObject _treePrefab;
    [SerializeField] private GameObject _mushroomPrefab;

    private int _itemSpace = 5;
    private int _itemCountInMap = 5;
    private int _mapSize;
    private float _LaneOffset = 2.2f;           // в playercontroller брать lane offset отсюда! 

    private int _mushroomCountInItem = 10;
    private float _mushroomHeight = 0.25f;

    enum TrackPos { Left = -1, Center = 0, Right = 1 };
    enum MushroomStyle { Empty, Line, Jump };

    struct MapItem
    {
        public void SetValues(GameObject obstacle, TrackPos trackPos, MushroomStyle mushroomStyle)
        {
            this.obstacle = obstacle;
            this.trackPos = trackPos;
            this.mushroomStyle = mushroomStyle;
        }
        public void SetValues(MushroomStyle mushroomStyle)
        {
            this.mushroomStyle = mushroomStyle;
        }

        public GameObject obstacle;
        public TrackPos trackPos;
        public MushroomStyle mushroomStyle;
    }

    public List<GameObject> maps = new List<GameObject>();
    public List<GameObject> activeMaps = new List<GameObject>();

    private void Awake()
    {
        _mapSize = _itemCountInMap * _itemSpace;

        maps.Add(MakeMap1());
        maps.Add(MakeMap2());
        maps.Add(MakeMap2());
        maps.Add(MakeMap3());
        maps.Add(MakeMap4());
        maps.Add(MakeMap5());
        maps.Add(MakeMap5());

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
        if (activeMaps[0].transform.position.z < -_mapSize)
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }
    }

    private void AddActiveMap()
    {
        int r = Random.Range(0, maps.Count);
        GameObject go = maps[r];
        go.SetActive(true);
        foreach (Transform child in go.transform)
        {
            child.gameObject.SetActive(true);
        }
        go.transform.position = activeMaps.Count > 0 ?
            activeMaps[activeMaps.Count - 1].transform.position + Vector3.forward * _mapSize :
            new Vector3(0, 0, 10); // если на сцене есть мапы - следующую смещаем на размер мапы, если нет
                                   // - перед ГГ смещаем на 20
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

    private GameObject MakeMap1()   // РОЩА ДЕРЕВЬЕВ
    {
        GameObject result = new GameObject();
        result.transform.SetParent(transform);  // чтобы на сцене result закидывался в иерархии к map generator 
        MapItem mapItem = new MapItem();

        for (int i = 0; i < _itemCountInMap; i++)
        {
            mapItem.SetValues(null, TrackPos.Center, MushroomStyle.Empty);

            if (i == 0) { mapItem.SetValues(_treePrefab, TrackPos.Center, MushroomStyle.Empty); }
            else if (i == 1) { mapItem.SetValues(_treePrefab, TrackPos.Left, MushroomStyle.Empty); }
            else if (i == 2) { mapItem.SetValues(_treePrefab, TrackPos.Right, MushroomStyle.Empty); }
            else if (i == 3) { mapItem.SetValues(_treePrefab, TrackPos.Center, MushroomStyle.Empty); }
            else if (i == 4) { mapItem.SetValues(_treePrefab, TrackPos.Right, MushroomStyle.Empty); }

            Vector3 obstaclePos = new Vector3((int)mapItem.trackPos * _LaneOffset, 0, i * _itemSpace);

            CreateMushrooms(mapItem.mushroomStyle, obstaclePos, result);

            if (mapItem.obstacle != null)
            {
                int isRandomRotation = Random.Range(0, 2);
                if (mapItem.obstacle == _treePrefab && isRandomRotation == 0)
                {
                    GameObject go = Instantiate(mapItem.obstacle, obstaclePos, Quaternion.identity);
                    go.transform.SetParent(result.transform);
                }
                else if (mapItem.obstacle == _treePrefab && isRandomRotation == 1)
                {
                    GameObject go = Instantiate(mapItem.obstacle, obstaclePos, new Quaternion(0, 180, 0, 0));
                    go.transform.SetParent(result.transform);
                }
            }
        }
        return result;
    }
    private GameObject MakeMap2()
    {
        GameObject result = new GameObject();
        result.transform.SetParent(transform);  // чтобы на сцене result закидывался в иерархии к map generator 
        MapItem mapItem = new MapItem();

        for (int i = 0; i < _itemCountInMap; i++)
        {
            mapItem.SetValues(null, TrackPos.Center, MushroomStyle.Empty);

            if (i == 0) { mapItem.SetValues(MushroomStyle.Line); }
            else if (i == 1) { mapItem.SetValues(_stumpPrefab, TrackPos.Left, MushroomStyle.Jump); }
            else if (i == 2) { mapItem.SetValues(_treePrefab, TrackPos.Right, MushroomStyle.Empty); }
            else if (i == 3) { mapItem.SetValues(MushroomStyle.Line); }
            else if (i == 4) { mapItem.SetValues(_treePrefab, TrackPos.Left, MushroomStyle.Empty); }

            Vector3 obstaclePos = new Vector3((int)mapItem.trackPos * _LaneOffset, 0, i * _itemSpace);

            CreateMushrooms(mapItem.mushroomStyle, obstaclePos, result);

            if (mapItem.obstacle != null)
            {
                GameObject go = Instantiate(mapItem.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }
    private GameObject MakeMap3()
    {
        GameObject result = new GameObject();
        result.transform.SetParent(transform);  // чтобы на сцене result закидывался в иерархии к map generator 
        MapItem mapItem = new MapItem();

        for (int i = 0; i < _itemCountInMap - 2; i++)
        {
            mapItem.SetValues(null, TrackPos.Center, MushroomStyle.Empty);

            if (i == 0) { mapItem.SetValues(_treePrefab, TrackPos.Center, MushroomStyle.Empty); }
            else if (i == 1) { mapItem.SetValues(_stumpPrefab, TrackPos.Right, MushroomStyle.Jump); }
            else if (i == 2) { mapItem.SetValues(_treePrefab, TrackPos.Left, MushroomStyle.Empty); }


            Vector3 obstaclePos = new Vector3((int)mapItem.trackPos * _LaneOffset, 0, 0);

            CreateMushrooms(mapItem.mushroomStyle, obstaclePos, result);

            if (mapItem.obstacle != null)
            {
                GameObject go = Instantiate(mapItem.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }
    private GameObject MakeMap4()
    {
        GameObject result = new GameObject();
        result.transform.SetParent(transform);  // чтобы на сцене result закидывался в иерархии к map generator 
        MapItem mapItem = new MapItem();

        for (int i = 0; i < _itemCountInMap - 2; i++)
        {
            mapItem.SetValues(null, TrackPos.Left, MushroomStyle.Empty);

            if (i == 0) { mapItem.SetValues(_treePrefab, TrackPos.Center, MushroomStyle.Empty); }
            else if (i == 1) { mapItem.SetValues(MushroomStyle.Line); }
            else if (i == 2) { mapItem.SetValues(_treePrefab, TrackPos.Right, MushroomStyle.Empty); }

            Vector3 obstaclePos = new Vector3((int)mapItem.trackPos * _LaneOffset, 0, 0);

            CreateMushrooms(mapItem.mushroomStyle, obstaclePos, result);

            if (mapItem.obstacle != null)
            {
                GameObject go = Instantiate(mapItem.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }
    private GameObject MakeMap5()
    {
        GameObject result = new GameObject();
        result.transform.SetParent(transform);  // чтобы на сцене result закидывался в иерархии к map generator 
        MapItem mapItem = new MapItem();

        for (int i = 0; i < _itemCountInMap; i++)
        {
            mapItem.SetValues(null, TrackPos.Center, MushroomStyle.Empty);

            if (i == 0) { mapItem.SetValues(_stumpPrefab, TrackPos.Left, MushroomStyle.Jump); }
            else if (i == 1) { mapItem.SetValues(MushroomStyle.Line); }
            else if (i == 2) { mapItem.SetValues(_treePrefab, TrackPos.Left, MushroomStyle.Empty); }
            else if (i == 3) { mapItem.SetValues(_treePrefab, TrackPos.Right, MushroomStyle.Empty); }
            else if (i == 4) { mapItem.SetValues(MushroomStyle.Line); }

            Vector3 obstaclePos = new Vector3((int)mapItem.trackPos * _LaneOffset, 0, i * _itemSpace);

            CreateMushrooms(mapItem.mushroomStyle, obstaclePos, result);

            if (mapItem.obstacle != null)
            {
                GameObject go = Instantiate(mapItem.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }

    private void CreateMushrooms(MushroomStyle style, Vector3 pos, GameObject parentObject)
    {
        Vector3 mushroomPos = Vector3.zero;

        if (style == MushroomStyle.Empty) { return; }

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
                mushroomPos.y = Mathf.Max(-1 / 6f * Mathf.Pow(i, 2) + (float)1.6, _mushroomHeight);
                mushroomPos.z = i * ((float)_itemSpace / _mushroomCountInItem);
                GameObject go = Instantiate(_mushroomPrefab, mushroomPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
    }

}
