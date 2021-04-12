using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using SimpleJSON;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class Week10 : MonoBehaviour
{
    /*
     * Create an object pool implementation.
     *
     * Requirements:
     *     - When prefab is added, create multiples and set them inactive
     *     - When Spawn() is called, either turn an inactive object on, or if there isn't one, spawn another
     *     - When Despawn() is called on an object, return it to the object pool
     *
     * Extra Ideas:
     *     Make a class that inherits from monobehavior to replace Start and Destroy
     *         - Initialization to replace "Start()"
     *         - Despawn to replace "GameObject.Destroy()"
     *         - Cleanup for replacing "OnDestroy()"
     *     Add position and rotation to Spawn()
     *     Allow there to be multiple kinds of prefabs tracked in the pool
     *     Make Despawn track what type of object was spawned/despawned and return to the right pool
     */

    public GameObject prefabToPool;
    public void Start()
    {
        ObjectPool.Add(prefabToPool);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ObjectPool.Spawn(prefabToPool);
        }
    }
}

public static class ObjectPool
{
    private static Dictionary<string, Stack<GameObject>> inactives = new Dictionary<string, Stack<GameObject>>();
    public static Dictionary<string, Transform> inactiveHolders = new Dictionary<string, Transform>();

    public static void Add(GameObject toPool, int numberToPreload = 20)
    {
        var inactiveHolder = new GameObject("--- OBJECTPOOL: " + toPool.name + " ---").transform;
        inactiveHolders.Add(toPool.name, inactiveHolder);

        inactives.Add(toPool.name, new Stack<GameObject>());

        for (var i = 0; i < numberToPreload; i++)
        {
            var gameObjectSpawned = GameObject.Instantiate(toPool);
            gameObjectSpawned.SetActive(false);
            gameObjectSpawned.transform.parent = inactiveHolder;
            gameObjectSpawned.name = toPool.name;
            inactives[toPool.name].Push(gameObjectSpawned);
        }
    }

    public static GameObject Spawn(GameObject pooledObject) => Spawn(pooledObject, null, Vector3.zero, Quaternion.identity);
    public static GameObject Spawn(GameObject pooledObject, Vector3 position) => Spawn(pooledObject, null, position, Quaternion.identity);
    public static GameObject Spawn(GameObject pooledObject, Transform parent, Vector3 position, Quaternion rotation)
    {
        if (!inactives.ContainsKey(pooledObject.name))
        {
            Debug.LogWarning("You need to call ObjectPool.Add() for this prefab - otherwise spawning will be slow.");
            Add(pooledObject, 1);
        }

        GameObject toSpawn;

        if (inactives[pooledObject.name].Count == 0)
        {
            toSpawn = UnityEngine.Object.Instantiate(pooledObject);
            toSpawn.name = pooledObject.name;
        }
        else
        {
            toSpawn = inactives[pooledObject.name].Pop();
        }

        toSpawn.SetActive(true);
        toSpawn.transform.parent = parent;
        toSpawn.transform.position = position;
        toSpawn.transform.rotation = rotation;
            
        var pooledComponents = toSpawn.GetComponents<PooledObject>();
        foreach (var component in pooledComponents)
            component.Initialize();

        return toSpawn;
    }

    public static void Despawn(GameObject toDespawn)
    {
        var pooledComponents = toDespawn.GetComponents<PooledObject>();
        foreach (var component in pooledComponents)
            component.CleanUp();

        if (!inactives.ContainsKey(toDespawn.name))
        {
            Debug.LogWarning("Attempt to despawn " + toDespawn.name + ", which was not spawned from a pool.");
            UnityEngine.Object.Destroy(toDespawn);
            return;
        }
        
        toDespawn.SetActive(false);
        toDespawn.transform.parent = inactiveHolders[toDespawn.name];
        inactives[toDespawn.name].Push(toDespawn);
    }
}

public class PooledObject : MonoBehaviour {
    public virtual void Initialize()
    {
        
    }

    public virtual void CleanUp()
    {
        
    }
}