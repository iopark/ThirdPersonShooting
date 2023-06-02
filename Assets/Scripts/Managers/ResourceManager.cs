using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public T Instantiate<T> (T original, Vector3 position, Quaternion rotation, bool pooling) where T : Object
    {
        if (pooling)
        {
            return GameManager.Pool.Get(original, position, rotation);
        }
        else
        {
            return Object.Instantiate(original, position, rotation);
        }
    }

    public void Destroy(GameObject go)
    {
        if (GameManager.Pool.Release(go))
            return;

        Debug.Log($"Destroying {go.name}");
        GameObject.Destroy(go);
    }
}
