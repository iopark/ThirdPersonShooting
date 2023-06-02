using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    // �÷��̾��� ���� ��ɸ� ���Խ�Ų��. MVC BABYYYYYYYYYYY
    // �������Ҷ��� �ִϸ��̼� ������ ���Ͽ� rig �� weight�� ���������� �����Ͽ��߸� �ϰڴ�. 
    // RayCast�� ���� ���� ���� 
    [SerializeField] Rig aimRig; 
    private Animator anim;
    private bool reloading;
    [SerializeField] private float reloadTime;

    // Any player is to have Weapon Holder, (which wields type of a gun) 
    [SerializeField] WeaponHolder weaponHolder;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        weaponHolder = GetComponentInChildren<WeaponHolder>(); // either this or place it manually through serialized field. 
    }

    private void OnReload(InputValue input)
    {
        if (reloading)
            return;
        StartCoroutine(ReloadRoutine()); 

    }

    IEnumerator ReloadRoutine()
    {
        
        anim.SetTrigger("Reload");
        reloading = true;
        //������ ���۽� weight �缳�� 
        aimRig.weight = 0f; 
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        aimRig.weight = 1f;

    }
    public void Fire()
    {
        weaponHolder.Fire();
        anim.SetTrigger("Fire");
    }

    private void OnFire(InputValue input)
    {
        if (reloading)
            return;
        Fire(); 
    }
}
