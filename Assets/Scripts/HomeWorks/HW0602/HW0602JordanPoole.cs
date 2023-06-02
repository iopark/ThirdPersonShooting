using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool; 

public class HW0602JordanPoole : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, ObjectPool<GameObject>>();
    }

    // Get 
    public T Get <T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        // IF GameOBJ  
        if (original is GameObject)
        {
            GameObject go = original as GameObject;

            if (!poolDictionary.ContainsKey(go.name))
            {
                CreatePoole(go.name, go); 
            }
            // 만약 반환하고자 하는 녀석이 이미 풀매니저에 있는 녀석이라면 

            ObjectPool<GameObject> pool = poolDictionary[go.name];
            go.transform.SetParent(parent); 
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.SetActive(true);
            return go as T;
        }
        // IF Component 
        else if (original is Component)
        {
            Component target = original as Component;
            
            if (!poolDictionary.ContainsKey(target.gameObject.name))
            {
                CreatePoole(target.gameObject.name, target.gameObject); 
            }

            ObjectPool<GameObject> pool = poolDictionary[target.gameObject.name];
            target.transform.SetParent(parent);
            target.transform.rotation = rotation;
            target.transform.position = position; 
            target.gameObject.SetActive(true);
            return target as T;
        }
        else
        {
            return null; 
        }
        
    }
    //Overloading for instances where there only T 
    public T Get<T>(T original) where T : Object
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity, null); 
    }
    //Overloading for instances where there only T, and Vector 

    public T Get<T>(T original, Vector3 position) where T : Object
    {
        return Get<T>(original, position, Quaternion.identity, null);
    }

    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object
    {
        return Get<T>(original, position, rotation, null);
    }
    /// <summary>
    /// Instances where identity is unique 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public T Get<T>(string value, Vector3 position, Quaternion rotation) where T : Object
    {
        string path = $"{typeof(T)}.{value}"; 

        T original = Resources.Load<T>(path);
        return Get<T>(original, position, rotation, null);
    }

    // Release 

    // CheckContain 

    // Initialize new ObjectPool

    private void CreatePoole(string key, GameObject prefab)
    {
        ObjectPool<GameObject> newObj = new ObjectPool<GameObject>(

            createFunc: () =>
            {
                GameObject go = Instantiate(prefab);
                go.name = key;
                return go;
            }, 
            actionOnGet: (GameObject go) =>
            {
                go.SetActive(true);
                go.transform.SetParent(null);
            }, 
            actionOnRelease: (GameObject go) =>
            {
                go.SetActive(false);
                go.transform.SetParent(transform); 
            }, 
            actionOnDestroy: (GameObject go) =>
            {
                Destroy(go);
            }, 
            true, 
            10, 
            200

            ) ;
    }
}
