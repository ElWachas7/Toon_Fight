using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour, IEnemy
{
    public float distance;
    public string name;
    public float Distance {get {return distance;} set { distance = value;}}
    public string Name => name;
}
