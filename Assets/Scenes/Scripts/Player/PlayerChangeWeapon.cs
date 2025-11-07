using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChangeWeapon : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] weaponList; // all weapon List sword / lance / bowNArrow
    [SerializeField] Image[] weaponListIm; 
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
            weaponListIm[Index].gameObject.SetActive(true);
        }
    }
    public void NextWeapon() 
    {
        weapon.gameObject.SetActive(false);
        weaponListIm[Index].gameObject.SetActive(false);
        Index = (Index + 1) % weaponList.Length;
        weapon = weaponList[Index].GetComponent<IWeapon>();
        weapon.gameObject.SetActive(true);
        weaponListIm[Index].gameObject.SetActive(true);
        
    }
   
    /*public void Attack() 
    {
        weapon?.Attack();
    }*/
}
