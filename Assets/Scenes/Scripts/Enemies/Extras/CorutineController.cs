using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorutineController : MonoBehaviour
{
    private static CorutineController _instance;

    public static CorutineController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("CorutineController");
                _instance = obj.AddComponent<CorutineController>();
            }
            return _instance;
        }
    }
}
