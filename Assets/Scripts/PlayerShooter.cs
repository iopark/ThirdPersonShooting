using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    // 플레이어의 슈팅 기능만 포함시킨다. MVC BABYYYYYYYYYYY
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnReload(InputValue input)
    {
        anim.SetTrigger("Reload"); 
    }

    private void OnFire(InputValue input)
    {
        anim.SetTrigger("Fire");
    }
}
