using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic;
    Dictionary<string, Transform> poolContainer;
    Transform poolRoot;

    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
        poolContainer = new Dictionary<string, Transform>();
        poolRoot = new GameObject("PoolRoot").transform;
    }

    /// <summary>
    /// Generic, T �� Object �̿���, �Ű������� �ִ� ���� ���ؼ� �ش� ���¸� ��ȯ�ϰ� �Ҽ� �ְ� �Ѵ�. , Where Obj �� ����Ƽ���� ���� �������� ��ü �����̴�. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="original"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object
    {
        // GameObject �϶�
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject; 
            if (!poolDic.ContainsKey(prefab.name))
            {
                CreatePool(prefab.name, prefab);
            }

            ObjectPool<GameObject> pool = poolDic[prefab.name]; // �ش� string ���� Ű������ �Ͽ� Dict���� ã�� ������Ʈ Ǯ�� ã�� ���Ŀ�, 
            GameObject go = pool.Get();  
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.transform.SetParent(transform); 
            return go as T;
        }
        // Component �϶� 
        if (original is Component)
        {
            Component component = original as Component; // T ����ȯ => Componenent
            string key = component.gameObject.name; 
            if (!poolDic.ContainsKey(key))
            {
                CreatePool(key, component.gameObject); // �ش� ������Ʈ�� �پ��ִ� GameObj �� Dict �� �߰��Ͽ��ָ�, 
            }                                   // 
            GameObject go = poolDic[key].Get(); // ���ӿ�����Ʈ�� �ҷ��µ�, 
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.transform.SetParent(transform);
            return go.GetComponent<T>(); // �ش� ���ӿ�����Ʈ�� ������Ʈ�� ��ȯ�ϴ� �������� ���ָ� �ȴ�. 
        }
        // GameObj ��, Componenent�� �ƴϸ� ����. �Դ°� �ƴϴ�. 
        else
        {
            return null; 
        }
    }
    //public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    //{
    //    if (!poolDic.ContainsKey(prefab.name))
    //    {
    //        CreatePool(prefab.name, prefab);
    //    }

    //    ObjectPool<GameObject> pool = poolDic[prefab.name]; // �ش� string ���� Ű������ �Ͽ� Dict���� ã�� ������Ʈ Ǯ�� ã�� ���Ŀ�, 

    //    GameObject go = pool.Get(); // ���� ù��° poolable obj �� ��ȯ�Ѵ�. 
    //    go.transform.position = position;
    //    go.transform.rotation = rotation;
    //    return go;
    //}

    public T Get<T>(T prefab) where T: Object
    {
        //if (!poolDic.ContainsKey(prefab.name))
        //{
        //    CreatePool(prefab.name, prefab);
        //}

        //ObjectPool<GameObject> pool = poolDic[prefab.name]; // �ش� string ���� Ű������ �Ͽ� Dict���� ã�� ������Ʈ Ǯ�� ã�� ���Ŀ�, 
        //// 
        //return pool.Get(); // ���� ù��° poolable obj �� ��ȯ�Ѵ�. 
        return Get(prefab, Vector3.zero, Quaternion.identity); // �����ε��� �̷��� �ϴ°��� �����. 
    }

    public bool IsContain<T>(T original) where T: Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;

        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    public bool Release(GameObject go)
    {
        if (!poolDic.ContainsKey(go.name))
        {
            return false; // ���ʿ� �ش� �ݳ��Ϸ��� ����� ���.name�� dict ���� ���� Ű���̶�� 
        }
        ObjectPool<GameObject> pool = poolDic[go.name];
        pool.Release(go); // �����ϰ� ��ȯ�Ѵ�. 
        return true; // �ݳ��� ������ ��� return true 
    }

    private void CreatePool(string key, GameObject prefab)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(

            //���鶧�� ������ Func, where Func does not require any Parameter values 
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.name = key; // �����Ǵ� ��� ��ü������ �̸��� Ű������ ���Ͻ��ϰ� �������ش�. 
                return obj;
            },
            // �����ö� Get() ������ Action 
            actionOnGet: (GameObject go) =>
            {
                go.SetActive(true);
                go.transform.parent = null;
            },
            // �ݳ��Ҷ� Release() ������ Action 
            actionOnRelease: (GameObject go) =>
            {
                go.SetActive(false);
                go.transform.SetParent(transform);
            },
            // �����ٶ� ������ Action 
            actionOnDestroy: (GameObject go) =>
            {
                Destroy(go);
            }
            );
        poolDic.Add(key, pool); // ���������� ���� �߰��� 
    }
}
