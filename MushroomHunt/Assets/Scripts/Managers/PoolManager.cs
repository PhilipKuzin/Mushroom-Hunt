using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonGeneric<PoolManager>
{
    class Pool
    {
        private List<GameObject> _inactive = new List<GameObject>();
        private GameObject prefab;

        public Pool(GameObject prefab)
        {
            this.prefab = prefab;
        }
        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            GameObject obj;
            if (_inactive.Count == 0)
            {
                obj = Instantiate(prefab, pos, rot);
                obj.name = prefab.name;
                obj.transform.SetParent(Instance.transform);
            }
            else
            {
                obj = _inactive[_inactive.Count - 1];
                _inactive.RemoveAt(_inactive.Count - 1);
            }
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);
            return obj;
        }

        public void Despawn(GameObject obj)
        {
            obj.SetActive(false);
            _inactive.Add(obj);
        }
    }

    private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    public void Preload (GameObject prefab, int num)
    {
        Init(prefab);
        GameObject[] objects = new GameObject[num];
        for (int i = 0;i <num; i++)
        {
            objects[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
        }
        for (int i = 0; i <num; i++)
        {
            Despawn(objects[i]);
        }
    }
    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        Init(prefab);
        return pools[prefab.name].Spawn(pos, rot);
    }

    public void Despawn(GameObject obj)
    {
        if (pools.ContainsKey(obj.name))
        {
            pools[obj.name].Despawn(obj);
        }
        else
        {
            Destroy(obj); 
        }
    }

    private void Init(GameObject prefab)
    {
        if (prefab != null && pools.ContainsKey(prefab.name) == false)
        {
            pools[prefab.name] = new Pool(prefab);
        }
    }
}
