using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] Poolable poolablePrefab;
    public int poolSize; 
    [SerializeField] int poolsize;
    [SerializeField] int maxSize;

    public Stack<Poolable> objectPool = new Stack<Poolable>();

    /// <summary>
    /// 외부상황에 영향을 받는다면 Start()
    /// 외부상황에 대해서 무관하다면 Awake()
    /// </summary>
    private void Awake()
    {
        CreatePool(); 
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolsize; i++)
        {
            Poolable poolable = Instantiate(poolablePrefab); 
            poolable.gameObject.SetActive(false);
            poolable.transform.SetParent(transform);
            poolable.Pool = this; 
            objectPool.Push(poolable);
        }
    }

    public Poolable Get()
    {
        if (objectPool.Count > 0)
        {
            Poolable poolable = objectPool.Pop();
            poolable.gameObject.SetActive(true);
            poolable.transform.parent = null;
            return poolable;
        }
        else
        {
            Poolable poolable = Instantiate(poolablePrefab);
            poolable.Pool = this;
            return poolable; 
        }
        
    }

    public void Release(Poolable pooled)
    {
        if (objectPool.Count < maxSize)
        {
            pooled.gameObject.SetActive(false);
            pooled.transform.SetParent(transform);
            objectPool.Push(pooled);
        }
        else
        {
            Destroy(pooled.gameObject);
        }
        poolSize = objectPool.Count;
    }
}
