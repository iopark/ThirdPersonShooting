using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static PoolManager poolinstance; 

    public static GameManager Instance
    {
        get { return instance; }
    }
    public static PoolManager Pool
    {
        get { return poolinstance; }
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
        poolinstance = poolObj.GetComponent<PoolManager>();
    }
}
