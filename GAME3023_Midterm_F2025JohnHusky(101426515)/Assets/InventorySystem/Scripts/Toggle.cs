using UnityEngine;

public class Toggle : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Inventory;
    public GameObject Weapon;
    public GameObject Armor;


    [Header("Sound FX")]
    /// SFX Toggle Methods
    /// add togge if you want sound effects when toggling between inventory, weapon, and armor or not
    public GameObject UISFXmain;
  



    public void OpenWeapon()
    {
        Inventory.SetActive(false);
        Weapon.SetActive(true);
        Armor.SetActive(false);

    }
    public void OpenArmor()
    {
        Inventory.SetActive(false);
        Weapon.SetActive(false);
        Armor.SetActive(true);

    }
    public void OpenInventory()
    {
        Inventory.SetActive(true);
        Weapon.SetActive(false);
        Armor.SetActive(false);

    }
}
