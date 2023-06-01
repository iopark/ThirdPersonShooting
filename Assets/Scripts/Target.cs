using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IHittable
{
    Rigidbody body;
    int health = 3; 
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    public void Hit(RaycastHit hit, int damage)
    {
        if(body  == null) return;
        health -= damage;
        body.AddForceAtPosition(hit.normal * -10, hit.point, ForceMode.Impulse); 
        if (health <= 0)
        {
            Destroy(gameObject); 
        }
    }
}
