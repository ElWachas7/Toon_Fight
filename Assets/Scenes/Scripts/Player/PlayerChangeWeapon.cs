using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerChangeWeapon : MonoBehaviour
{
    [SerializeField] CapsuleCollider AreaDeAtaque; // hit area
    [SerializeField] MeshRenderer playerSprite; // hit area
    [SerializeField] MonoBehaviour[] weaponList; // all weapon List sword / lance / bowNArrow
    private int Index = 0;
    private IWeapon weapon; // weapon in hand
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            Attack();
            
        }
        else if (Input.GetKeyDown(KeyCode.Q)) 
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
            SetWeaponStats();
        }
    }
    public void NextWeapon() 
    {
        Index = (Index + 1) % weaponList.Length;
        weapon = weaponList[Index].GetComponent<IWeapon>();
        SetWeaponStats();
    }
    public void SetWeaponStats() 
    {
        if (weapon != null)
        {
            AreaDeAtaque.radius = weapon.Radius;
            playerSprite.material = weapon.Material;
        }
    }
    public void Attack() 
    {
        weapon?.Attack();
    }
}
