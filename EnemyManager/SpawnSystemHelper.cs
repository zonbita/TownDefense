﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystemHelper : MonoBehaviour {
	static public GameObject GetNextObject(GameObject sourceObj, bool activateObject = true)
	{
		int uniqueId = sourceObj.GetInstanceID();

		if(!instance.poolCursors.ContainsKey(uniqueId))
		{
			Debug.LogError("[CFX_SpawnSystem.GetNextPoolObject()] Object hasn't been preloaded: " + sourceObj.name + " (ID:" + uniqueId + ")");
			return null;
		}

		int cursor = instance.poolCursors[uniqueId];
		instance.poolCursors[uniqueId]++;
		if(instance.poolCursors[uniqueId] >= instance.instantiatedObjects[uniqueId].Count)
		{
			instance.poolCursors[uniqueId] = 0;
		}

		GameObject returnObj = instance.instantiatedObjects[uniqueId][cursor];
		if(activateObject)
			#if UNITY_3_5
			returnObj.SetActiveRecursively(true);
			#else
            if(returnObj)
			returnObj.SetActive(true);
			#endif

		return returnObj;
	}

	static public void PreloadObject(GameObject sourceObj, int poolSize = 1)
	{
		instance.addObjectToPool(sourceObj, poolSize);
	}

	static public void UnloadObjects(GameObject sourceObj)
	{
		instance.removeObjectsFromPool(sourceObj);
	}

	static public bool AllObjectsLoaded
	{
		get
		{
			return instance.allObjectsLoaded;
		}
	}

	static private SpawnSystemHelper instance;

	public GameObject[] objectsToPreload = new GameObject[0];
	public int[] objectsToPreloadTimes = new int[0];
	public bool hideObjectsInHierarchy;

	private bool allObjectsLoaded;
	private Dictionary<int,List<GameObject>> instantiatedObjects = new Dictionary<int, List<GameObject>>();
	private Dictionary<int,int> poolCursors = new Dictionary<int, int>();

	private void addObjectToPool(GameObject sourceObject, int number)
	{
		int uniqueId = sourceObject.GetInstanceID();

		if(!instantiatedObjects.ContainsKey(uniqueId))
		{
			instantiatedObjects.Add(uniqueId, new List<GameObject>());
			poolCursors.Add(uniqueId, 0);
		}
		
		GameObject newObj;
		for(int i = 0; i < number; i++)
		{
			newObj = (GameObject)Instantiate(sourceObject, new Vector2(0,100),sourceObject.transform.rotation);
			#if UNITY_3_5
			newObj.SetActiveRecursively(false);
			#else
			newObj.SetActive(false);
			#endif

			instantiatedObjects[uniqueId].Add(newObj);

			if(hideObjectsInHierarchy)
				newObj.hideFlags = HideFlags.HideInHierarchy;
		}
	}

	private void removeObjectsFromPool(GameObject sourceObject)
	{
		int uniqueId = sourceObject.GetInstanceID();

		if(!instantiatedObjects.ContainsKey(uniqueId))
		{
			Debug.LogWarning("[CFX_SpawnSystem.removeObjectsFromPool()] There aren't any preloaded object for: " + sourceObject.name + " (ID:" + uniqueId + ")");
			return;
		}

		//Destroy all 
		for(int i = instantiatedObjects[uniqueId].Count - 1; i >= 0; i--)
		{
			GameObject obj = instantiatedObjects[uniqueId][i];
			instantiatedObjects[uniqueId].RemoveAt(i);
			GameObject.Destroy(obj);
		}

		//Remove pool 
		instantiatedObjects.Remove(uniqueId);
		poolCursors.Remove(uniqueId);
	}

	void Awake()
	{
		if(instance != null)
			Debug.LogWarning("CFX_SpawnSystem: There should only be one instance of CFX_SpawnSystem per Scene!");

		instance = this;
	}

	void Start()
	{
		allObjectsLoaded = false;

		for(int i = 0; i < objectsToPreload.Length; i++)
		{
			PreloadObject(objectsToPreload[i], objectsToPreloadTimes[i]);
		}

		allObjectsLoaded = true;
	}
}
