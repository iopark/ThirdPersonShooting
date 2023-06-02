using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static PoolManager pool;
    private static ResourceManager resource;

    public static GameManager Instance
    {
        get { return instance; }
    }
    public static PoolManager Pool
    {
        get { return pool; }
    }
    public static ResourceManager Resource
    {
        get { return resource; }
    }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Following has been established Already"); 
            Destroy(this);
            return; 
        }
        instance = this;
        DontDestroyOnLoad(this);
        InitManagers(); 
    }

    private void InitManagers()
    {
        GameObject poolObj = new GameObject() { name = "Pool Manager" };
        poolObj.transform.SetParent(transform); 
        pool = poolObj.AddComponent<PoolManager>();

        GameObject resourceObj = new GameObject() { name = "Resource Manager" }; 
        resourceObj.transform.SetParent(transform);
        resource = resourceObj.AddComponent<ResourceManager>();
    }
}
