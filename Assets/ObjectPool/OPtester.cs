using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPtester : MonoBehaviour
{
    private ObjectPool pool;

    private void Awake()
    {
        pool = GetComponent<ObjectPool>();  
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            Poolable poolable = pool.Get();
            pool.transform.position = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f)); 
        }
    }
}
