using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    [Header("Aspectos visuales")]
    [SerializeField] private MeshRenderer cross1; // son uno mismo, componen la cruz de la base
    [SerializeField] private MeshRenderer cross2;
    [SerializeField] private Material CanBuy;
    [SerializeField] private Material FarAway;
    [SerializeField] private Material noMoney; //suficientemente cerca pero no puede comprarlo
    [SerializeField] private Canvas Button;
    [SerializeField] private Image RedRectE;//barra lateral roja para mostrar que no se puede comprar
    [SerializeField] private Image RedRectR;

    [Header("Configuracion visual")]
    private float amplitude = 0.2f; //cuanto sube y baja
    private float frequency = 2f; //velocidad
    private Vector3 startPos;
    private bool inRangeToBuy; //significa que esta en rango de poder comprar

    [Header("Tower Stats")]
    [SerializeField] public GameObject ArrowTower;
    [SerializeField] public GameObject StoneTower;
    [SerializeField] public GameObject UnBuild;
    [SerializeField] public int Price;
    public TowerData ArrowData;
    public TowerData StoneData;


    public void Awake() // aspectos y configuracion visual
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

        if (GameManager.gameManagerSingleton.money < Price)
        {
            cross1.material = noMoney; //no se puede comprar
            cross2.material = noMoney;
            RedRectE.gameObject.SetActive(true);
            RedRectR.gameObject.SetActive(true);
        }
        else
        {
            cross1.material = CanBuy; // se puede comprar
            cross2.material = CanBuy;
            RedRectE.gameObject.SetActive(false);
            RedRectR.gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeTower(ArrowTower);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                ChangeTower(StoneTower);
            }
        }
    }
    public void ChangeTower(GameObject tower)
    {
        tower.SetActive(true);
        UnBuild.gameObject.SetActive(false);
        GameManager.gameManagerSingleton.money -= Price;
        inRangeToBuy = false;
    }
}
