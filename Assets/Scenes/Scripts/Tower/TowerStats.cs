using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
    //Arrow
    public int damage;
    public float projectileSpeed;
    //tower
    public float range;
    public float cooldown;
    //status
    public int exp;
    public int level;

    public void ChargeStats(TowerData data)
    {
        damage = data.damage;
        projectileSpeed = data.projectileSpeed;
        range = data.range;
        cooldown = data.cooldown;
        exp = data.exp;
        level = data.level;
    }
}
