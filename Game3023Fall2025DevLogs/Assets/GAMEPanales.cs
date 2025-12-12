using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAMEPanales : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject INV;
    public GameObject SHOP;
    public GameObject ACH;
    public GameObject SETT;

    // Close all panels
    private void CloseAll()
    {
        INV.SetActive(false);
        SHOP.SetActive(false);
        ACH.SetActive(false);
        SETT.SetActive(false);
    }

    // Toggle Inventory
    public void INVOC()
    {
        bool toggle = !INV.activeSelf;  // If open, close. If closed, open.
        CloseAll();
        INV.SetActive(toggle);
    }

    // Toggle Shop
    public void SHOPVOC()
    {
        bool toggle = !SHOP.activeSelf;
        CloseAll();
        SHOP.SetActive(toggle);
    }

    // Toggle Achievements
    public void ACHVOC()
    {
        bool toggle = !ACH.activeSelf;
        CloseAll();
        ACH.SetActive(toggle);
    }

    // Toggle Settings
    public void SETTOC()
    {
        bool toggle = !SETT.activeSelf;
        CloseAll();
        SETT.SetActive(toggle);
    }
}