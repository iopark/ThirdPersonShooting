using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HW0601_WeaponHolder : MonoBehaviour
{
    // WeaponHolder can wield any guns 
    [SerializeField] HW0601_Gun gun; 


    public void Fire()
    {
        gun.Fire();
    }
}
