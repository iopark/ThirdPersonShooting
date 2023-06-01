using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    // WeaponHolder can wield any guns 
    [SerializeField] Gun gun; 

    List<Gun> gunList = new List<Gun>();

    public void Swap(int index)
    {
        gun = gunList[index];
    }
    public void Fire()
    {
        gun.Fire();
    }

    public void GetWeapon(Gun gun)
    {
        gunList.Add(gun);
    }
}
