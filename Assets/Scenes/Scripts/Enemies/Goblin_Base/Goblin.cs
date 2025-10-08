using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour, IEnemy
{
    public float distance;
    public float Distance {get {return distance;} set { distance = value;}}
}
