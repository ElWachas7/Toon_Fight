using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChangeWeapon : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] weaponList; // all weapon List sword / lance / bowNArrow
    [SerializeField] Image[] weaponListIm; //Para la UI
    private int index = 0;
    public int Index => index;//animator, index = weapon
    private IWeapon weapon; // weapon in hand
    public IWeapon Weapon => weapon;
    
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
            weapon = weaponList[index].GetComponent<IWeapon>();
            weapon.gameObject.SetActive(true);
            weaponListIm[index].gameObject.SetActive(true);
        }
    }
    public void NextWeapon() 
    {
        weapon.gameObject.SetActive(false);
        weaponListIm[index].gameObject.SetActive(false);
        index = (index + 1) % weaponList.Length;
        weapon = weaponList[index].GetComponent<IWeapon>();
        weapon.gameObject.SetActive(true);
        weaponListIm[index].gameObject.SetActive(true);
        
    }
   
    /*public void Attack() 
    {
        weapon?.Attack();
    }*/
}
