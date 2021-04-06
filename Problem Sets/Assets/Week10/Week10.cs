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
     *         - Despawn to replace "Destroy()"
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

    public static void Add(GameObject toPool, int numberToPreload = 20)
    {
        // Add type of gameobject to pool and spawn
    }

    public static GameObject Spawn(GameObject pooledObject) => Spawn(pooledObject, null, Vector3.zero, Quaternion.identity);
    public static GameObject Spawn(GameObject pooledObject, Vector3 position) => Spawn(pooledObject, null, position, Quaternion.identity);

    public static GameObject Spawn(GameObject pooledObject, Transform parent, Vector3 position, Quaternion rotation)
    {
        // Get gameobject from pool
        
        return new GameObject();
    }

    public static void Despawn(GameObject toDespawn)
    {
        // Return gameobject to pool
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