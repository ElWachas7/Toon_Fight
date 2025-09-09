using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowNArrow : MonoBehaviour , IWeapon
{
    public void Attack() 
    {
        Debug.Log("Shooting an arrow");
    }
}
