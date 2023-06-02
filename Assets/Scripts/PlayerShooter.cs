using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    // 플레이어의 슈팅 기능만 포함시킨다. MVC BABYYYYYYYYYYY
    // 재장전할때의 애니메이션 구현을 위하여 rig 의 weight를 인위적으로 조작하여야만 하겠다. 
    // RayCast를 통한 슈팅 구현 
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
        //재장전 시작시 weight 재설정 
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
