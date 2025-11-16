
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
    private float amplitude = 0.2f; //cuanto sube y baja
    private float frequency = 2f; //velocidad
    private Vector3 startPos;

    
    private bool inRangeToBuy; //significa que esta en rango de poder comprar
    [SerializeField] public GameObject ArrowTower;
    [SerializeField] public GameObject StoneTower;
    [SerializeField] public int Price;


    public void Awake()
    {
        startPos = Button.transform.position;
        cross1.material = FarAway;
        cross2.material = FarAway;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            inRangeToBuy = true;
            Button.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            inRangeToBuy = false;
            cross1.material = FarAway;
            cross2.material = FarAway;
            Button.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (inRangeToBuy)
        { ChangeColor(); }
    }
    private void ChangeColor()
    {
        // Movimiento senoidal vertical
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        Button.transform.position = new Vector3(startPos.x, newY, startPos.z);
        if (GameManager.gameManagerSingleton.Money < Price)
        {
            cross1.material = noMoney; //no se puede comprar
            cross2.material = noMoney;
            RedRect.gameObject.SetActive(true);
        }
        else
        {
            cross1.material = CanBuy; // se puede comprar
            cross2.material = CanBuy;
            RedRect.gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                ArrowTower.SetActive(true);
                this.gameObject.SetActive(false);
                GameManager.gameManagerSingleton.Money -= Price;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                StoneTower.SetActive(true);
                this.gameObject.SetActive(false);
                GameManager.gameManagerSingleton.Money -= Price;
            }
        }
    }
}
