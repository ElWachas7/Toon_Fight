using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerChangeWeapon : MonoBehaviour
{
    [SerializeField] MeshRenderer playerSprite; // hit area
    [SerializeField] MonoBehaviour[] weaponList; // all weapon List sword / lance / bowNArrow
    private int Index = 0;
    private IWeapon weapon; // weapon in hand
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            NextWeapon();
        }
    }
     void Awake()
    {
        if (weaponList.Length > 0)
        {
            // get the first weapon in hand
            weapon = weaponList[Index].GetComponent<IWeapon>();
            weapon.gameObject.SetActive(true);
            SetWeaponStats();
        }
    }
    public void NextWeapon() 
    {
        weapon.gameObject.SetActive(false);
        Index = (Index + 1) % weaponList.Length;
        weapon = weaponList[Index].GetComponent<IWeapon>();
        weapon.gameObject.SetActive(true);
        SetWeaponStats();
    }
    public void SetWeaponStats() 
    {
        if (weapon != null)
        {
            playerSprite.material = weapon.Material;
        }
    }
    /*public void Attack() 
    {
        weapon?.Attack();
    }*/
}
