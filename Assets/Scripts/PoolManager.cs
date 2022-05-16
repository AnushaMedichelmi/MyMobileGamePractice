using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A struct for objects to be pooled.
[System.Serializable]
public class ObjectToPool
{
    public GameObject prefab;
    public int initialCapacity;
}
public class PoolManager : MonoBehaviour
{
	#region PUBLIC VARIABLES
	// Objects to be pooled at initialization.
	public ObjectToPool[] prefabsToPool;
	#endregion

	#region PRIVATE VARIABLES
	private Dictionary<string, ObjectPool> pools;
	#endregion

	#region SINGLETON

	private static PoolManager instance;
	public static PoolManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<PoolManager>();
				if (instance == null)
				{
					GameObject container = new GameObject("PoolManager");
					instance = container.AddComponent<PoolManager>();
				}
			}
			return instance;
		}
	}
	#endregion

	void Start()
	{
		for (int i = 0; i < prefabsToPool.Length; i++)
		{
			CreatePool(prefabsToPool[i].prefab, prefabsToPool[i].initialCapacity);
		}
	}


	#region PUBLIC METHODS
	// Create a new pool of objects at runtime.
	public void CreatePool(GameObject prefab, int initialCapacity)
	{
		if (pools == null)
			pools = new Dictionary<string, ObjectPool>();                 //Dictionary are in great use when we have large list

		ObjectPool newPool = new ObjectPool(prefab, initialCapacity);
		pools.Add(prefab.name, newPool);
	}

	// Spawn an object with the given name.
	public GameObject Spawn(string prefabName)
	{
		if (!pools.ContainsKey(prefabName))
			return null;

		return pools[prefabName].Spawn();
	}

	// Recycle an object with the given name.
	public void Recycle(string prefabName, GameObject obj)
	{
		if (!pools.ContainsKey(prefabName))
			return;

		pools[prefabName].Recycle(obj);
	}
	#endregion
}
