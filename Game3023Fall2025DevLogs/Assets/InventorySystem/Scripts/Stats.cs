using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// self explanatory, stats store player stats and update UI
public class StatsScript : MonoBehaviour
{
    [Header("Default Stats")]
    public int STR = 0;
    public int DEX = 0;
    public int INT = 0;
    public int DEF = 0;
    public int STA = 0;
    public int MANA = 50;
    public int HEALTH = 100;
    public int LVL = 1;
    public string ItemName = "Item Name";
    public string ItemDIS = "This is an item description";

    [Header("UI Text References (TMP)")]
    public TextMeshProUGUI STR_Text;
    public TextMeshProUGUI DEX_Text;
    public TextMeshProUGUI INT_Text;
    public TextMeshProUGUI DEF_Text;
    public TextMeshProUGUI STA_Text;
    public TextMeshProUGUI MANA_Text;
    public TextMeshProUGUI HEALTH_Text;
    public TextMeshProUGUI LVL_Text;


    [Header("Item Name & Description UI")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        // Continually update UI in case stats change from other scripts
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (STR_Text != null) STR_Text.text = STR.ToString();
        if (DEX_Text != null) DEX_Text.text = DEX.ToString();
        if (INT_Text != null) INT_Text.text = INT.ToString();
        if (DEF_Text != null) DEF_Text.text = DEF.ToString();
        if (STA_Text != null) STA_Text.text = STA.ToString();
        if (MANA_Text != null) MANA_Text.text = MANA.ToString();
        if (HEALTH_Text != null) HEALTH_Text.text = HEALTH.ToString();
        if (LVL_Text != null) LVL_Text.text = LVL.ToString();
//if (itemNameText != null) itemNameText.text = ItemName;
       // if (itemDescriptionText != null) itemDescriptionText.text = ItemDIS;
    }
}

