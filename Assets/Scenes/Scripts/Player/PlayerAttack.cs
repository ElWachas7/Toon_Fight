using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] CapsuleCollider AreaDeAtaque;
    [SerializeField] IWeapon[] ListaDeWeapons;
    private int Index = 0;
    private int nextIndex;
    private IWeapon weapon; // weapon en mano
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            Attack();
            
        }
        if (weapon == null)
        {
            Debug.Log("There's no weapon yet");
        }
    }

     void Awake()
    {
        if(ListaDeWeapons.Length > 0) 
        {
            weapon = ListaDeWeapons[Index] as IWeapon;
        }    
    }

    public void SwitchWeapon(int Index) 
    {
        if (Index < 0 || Index >= ListaDeWeapons.Length) return;

        weapon = ListaDeWeapons[Index] as IWeapon;
        this.Index = Index; // si este no funciona lo cambiamos
    }

    public void NextWeapon() 
    {
        nextIndex = (Index + 1) % ListaDeWeapons.Length;
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
