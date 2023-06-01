using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class HW0601_PlayerShooter : MonoBehaviour
{
    // �÷��̾��� ���� ��ɸ� ���Խ�Ų��. MVC BABYYYYYYYYYYY
    // �������Ҷ��� �ִϸ��̼� ������ ���Ͽ� rig �� weight�� ���������� �����Ͽ��߸� �ϰڴ�. 
    // RayCast�� ���� ���� ���� 
    [SerializeField] Rig aimRig; 
    private Animator anim;
    private bool reloading;
    [SerializeField] private WaitForSeconds reloadTime = new WaitForSeconds(2.5f); 

    // Any player is to have Weapon Holder, (which wields type of a gun) 
    [SerializeField] HW0601_WeaponHolder weaponHolder;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        weaponHolder = GetComponentInChildren<HW0601_WeaponHolder>(); // either this or place it manually through serialized field. 
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
        yield return reloadTime; 
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
