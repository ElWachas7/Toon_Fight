using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] CapsuleCollider AreaDeAtaque;
    [SerializeField] GameObject[] ListaDeWeapons;
    private int Index = 0;
    private int nextIndex;
    private IWeapon weapon; // weapon en mano
    
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
        if (ListaDeWeapons.Length > 0)
        {
            // obtener el primer arma
            weapon = ListaDeWeapons[Index].GetComponent<IWeapon>();
            ListaDeWeapons[Index].SetActive(true);
        }
    }
    public void NextWeapon() 
    {
        ListaDeWeapons[Index].SetActive(false);
        Index = (Index + 1) % ListaDeWeapons.Length;
        ListaDeWeapons[Index].SetActive(true);
        weapon = ListaDeWeapons[Index].GetComponent<IWeapon>();
    }

    public void Attack() 
    {
        weapon?.Attack();
    }
    public void SetWeapon(IWeapon weapon) //para agarrar armas del suelo como gameObjetcs
    {
        this.weapon = weapon;
    }
}
