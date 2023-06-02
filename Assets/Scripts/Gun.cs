using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] float bulletSpeed; 
    // Generally speaking, this should be done on the target being hit by the target . 

    [SerializeField] float maxDistance;
    [SerializeField] float fireRate; // defined by the time, (s) 
    [SerializeField] int damage;

    private void Start()
    {
        damage = 1;
        bulletSpeed = 50; 
        maxDistance = 100;
    }
    public void Fire()
    {
        muzzleEffect.Play(); // if particle is owned by the object, simply play this. 
        
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            //����Ʈ�� ���ؼ� ������Ʈ Ǯ������ ���� 
            IHittable hittableObj = hit.transform.GetComponent<IHittable>(); // Interface�� Componenentó�� ����� �����ϴ�: how crazy is that;
            //ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(effect.gameObject, 3f);
            ParticleSystem effect = GameManager.Pool.Get(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //effect.transform.position = hit.point;
            //effect.transform.rotation = Quaternion.LookRotation(hit.normal);
            //effect.transform.parent = hit.transform; 
            //effect.transform.SetParent(hit.transform); // To make the particle effect follow the struck target 
            StartCoroutine(ReleaseRoutine(effect.gameObject));


            //Where Quaternion.identity means no rotation value at all 
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, hit.point));




            hittableObj?.Hit(hit, damage); // if ain't null, proceed with Hit, else, return; 
            // where 
        }
        else
        {
            
            //Where Quaternion.identity means no rotation value at all 
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));

        }
    }

    IEnumerator ReleaseRoutine(GameObject effect)
    {
        yield return new WaitForSeconds(3f); 
        GameManager.Pool.Release(effect); // Instead of Destroy, simply return it to the ObjectPool within the Dict 
    }

    IEnumerator TrailRoutine(Vector3 startPoint, Vector3 endPoint)
    {
        //TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
        TrailRenderer trail = GameManager.Pool.Get(bulletTrail, startPoint, Quaternion.identity); 
        trail.Clear(); 

        float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed;

        float rate = 0; 
        while (rate <1)
        {
            trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate); // ���������� �������� �ð��� ������ �̵��ϴµ�, 
            rate += Time.deltaTime / totalTime; // �ش� �ð��� �� �ݺ����� deltatime/ total time ���� ������Ų��. 

            yield return null; 
        }
        //Destroy(trail.gameObject, 3f);
        GameManager.Pool.Release(trail.gameObject);

        yield return null;

        if (!trail.IsValid())
        {
            Debug.Log("Ʈ������ ����"); 
        }
        else
        {
            Debug.Log("Ʈ������ �ִ�"); 
        }
    }
}