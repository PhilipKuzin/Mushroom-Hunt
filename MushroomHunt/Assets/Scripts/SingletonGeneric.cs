using UnityEngine;

public abstract class SingletonGeneric<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            T[] instances = FindObjectsOfType<T>();

            if (instances.Length > 0)
            {
                _instance = instances[0];

                for (int i = 1; i < instances.Length; i++)
                {
                    Destroy(instances[i]);
                }
            }
            else
            {
                GameObject go = new GameObject();
                go.name = typeof(T).ToString();
                _instance = go.AddComponent<T>();
            }
            DontDestroyOnLoad(_instance);
            return _instance;
        }
    }
}
