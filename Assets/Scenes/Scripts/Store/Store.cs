using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public TowerData ArrowData;
    public TowerData StoneData;

    [Header("Textos")]
    [SerializeField] TextMeshProUGUI ATDmg;
    [SerializeField] TextMeshProUGUI ATRng;
    [SerializeField] TextMeshProUGUI ATCol;
    [SerializeField] TextMeshProUGUI STDmg;
    [SerializeField] TextMeshProUGUI STRng;
    [SerializeField] TextMeshProUGUI STcol;
    [SerializeField] private int price;
    private bool canBuy;
    //damage , cooldown , range , projectileSpeed

    public void Update()
    {
        ATDmg.text = ArrowData.damage.ToString("0");
        ATRng.text = ArrowData.range.ToString("0");
        ATCol.text = ArrowData.cooldown.ToString("F1");
        STDmg.text = StoneData.damage.ToString("0");
        STRng.text = StoneData.range.ToString("0");
        STcol.text = StoneData.cooldown.ToString("F1");
        if(GameManager.gameManagerSingleton.money < price) 
        {
            canBuy = false;
        }
        else 
        {
            canBuy = true;  
        }
    }
    public void ArrowDmg() 
    {
        if (canBuy) 
        {
            ArrowData.damage += 10;
            GameManager.gameManagerSingleton.money -= price;
        }
         
    }
    public void StoneDmg() { if (canBuy) { StoneData.damage += 10; GameManager.gameManagerSingleton.money -= price; } }
    public void ArrowRan() { if (canBuy) { ArrowData.range += 1; GameManager.gameManagerSingleton.money -= price; } }
    public void StoneRan() { if (canBuy) {StoneData.range += 1; GameManager.gameManagerSingleton.money -= price; } }
    public void ArrowCoolDown() { if (canBuy) { ArrowData.cooldown -= 0.1f; ArrowData.projectileSpeed += 0.5f; GameManager.gameManagerSingleton.money -= price; } }
    public void StoneCoolDown() { if (canBuy) { StoneData.cooldown -= 0.1f; StoneData.projectileSpeed += 0.5f; GameManager.gameManagerSingleton.money -= price; } }
}
