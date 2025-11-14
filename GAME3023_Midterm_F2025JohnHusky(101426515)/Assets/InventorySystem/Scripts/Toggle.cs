using TMPro;
using UnityEngine;
// self explanatory
public class Toggle : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Inventory;
    public GameObject Weapon;
    public GameObject Cosmetics;



    [Header("Sound FX")]
    /// SFX Toggle Methods
    /// add togge if you want sound effects when toggling between inventory, weapon, and armor or not
    public GameObject UISFXmain;
  



    public void OpenWeapon()
    {
        Inventory.SetActive(false);
        Weapon.SetActive(true);
        Cosmetics.SetActive(false);

    }
    public void OpenArmor()
    {
        Inventory.SetActive(false);
        Weapon.SetActive(false);
        Cosmetics.SetActive(true);

    }
    public void OpenInventory()
    {
        Inventory.SetActive(true);
        Weapon.SetActive(false);
        Cosmetics.SetActive(false);

    }

}
