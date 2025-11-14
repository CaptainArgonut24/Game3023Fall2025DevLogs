using TMPro;
using UnityEngine;
// to make an item scriptable object via code inseded of just a prefab
//Attribute which allows right click->Create
[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item : ScriptableObject //Extending SO allows us to have an object which exists in the project, not in the scene
{

    [Header("Item Info")]
    public int count = 0;

    public Sprite icon;
    public string description = "";
    public bool isConsumable = false;

    public int width = 1;
    public int height = 1;
    public string itemName = "";


    [Header("Swap Settings")]
    public bool allowItemSwap = true;

    [HideInInspector] public Transform parentAfterDrag;

    private bool isDragging = false;
    private Transform originalSlot; // <-- store the original slot

   
    

    public void Use()
    {
        Debug.Log("Used item: " + name + " - " + description);
    }

}
