using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonGeneric <T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance = null;
	public static T Instance
	{
		get 
		{ 
			if(_instance != null)
				return _instance;
			T[] instances = FindObjectsOfType<T>();
			if (instances.Length > 0)
			{
				_instance = instances[0];
				for (int i = 1; i < instances.Length; i++)
				{
					Destroy(instances[i]);
				}
			}
			DontDestroyOnLoad(_instance);
			return _instance;
		}		
	}
}
