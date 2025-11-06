
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnBuild : MonoBehaviour
{
    [SerializeField] private MeshRenderer cross1; // son uno mismo, componen la cruz de la base
    [SerializeField] private MeshRenderer cross2;

    [SerializeField] private Material CanBuy;
    [SerializeField] private Material FarAway;
    [SerializeField] private Material noMoney; //suficientemente cerca pero no puede comprarlo

    [SerializeField] private Canvas Button;
    [SerializeField] private Image RedRect;//barra lateral roja para mostrar que no se puede comprar

    public PlayerEconomy playerEconomy;
    private bool chupeteInside; //significa que esta en rango de poder comprar
    [SerializeField] public GameObject Builded;


    public void Awake()
    {
        cross1.material = FarAway;
        cross2.material = FarAway;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            chupeteInside = true;
            Button.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            chupeteInside = false;
            cross1.material = FarAway;
            cross2.material = FarAway;
            Button.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (chupeteInside)
        { ChangeColor(); }
    }
    private void ChangeColor()
    {
        if (playerEconomy.Money <= 0)
        {
            cross1.material = noMoney;
            cross2.material = noMoney;
            RedRect.gameObject.SetActive(true);
        }
        else
        {
            cross1.material = CanBuy;
            cross2.material = CanBuy;
            RedRect.gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                Builded.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}
