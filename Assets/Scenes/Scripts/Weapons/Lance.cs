using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour , IWeapon
{
    public void Attack()
    {
        Debug.Log("using lance");
    }
}
