using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tent : MonoBehaviour
{
    public GameObject store;
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entro a la tienda");
        store.gameObject.SetActive(true);
    }
    public void OnTriggerExit(Collider other)
    {
        store.gameObject.SetActive(false);
    }
}
