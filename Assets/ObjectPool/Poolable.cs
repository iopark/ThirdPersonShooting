using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class Poolable : MonoBehaviour
    {
        // Component attachable to the prefab used as a poolable. 
        [SerializeField] float releaseTime; // return timing 
        private ObjectPool pool;

        public ObjectPool Pool { get { return pool; } set { pool = value; } }

        private void OnEnable()
        {
            releaseTime = 3;
            StartCoroutine(ReleaseTimer());
        }

        IEnumerator ReleaseTimer()
        {
            yield return new WaitForSeconds(releaseTime);
            pool.Release(this);
        }
    }
}

