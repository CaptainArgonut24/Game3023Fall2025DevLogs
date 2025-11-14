using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemEntry
{
    public GameObject itemPrefab;   // The item UI object to spawn
    public int quantity = 1;        // How many copies to place
}

public class InventoryHandleler : MonoBehaviour
{
    [Header("Slot Settings")]
    [Tooltip("All UI slot GameObjects where items will be placed.")]
    public List<GameObject> itemSlots = new List<GameObject>();

    [Header("Item Settings")]
    [Tooltip("Item prefabs and how many copies to generate.")]
    public List<InventoryItemEntry> itemEntries = new List<InventoryItemEntry>();

    [Tooltip("How many empty slots you want to keep.")]
    public int emptySlotCount = 0;

    [Header("Placement Settings")]
    [Tooltip("Randomly distribute items across slots, else ordered.")]
    public bool randomPlacement = false;

    [Tooltip("If TRUE, inventory pulls from an external data source.")]
    public bool useExternalDataSource = false;

    [Tooltip("Optional external data object to override itemEntries.")]
    public InventoryExternalData externalData;

    private List<GameObject> generatedItems = new List<GameObject>();

    private void Start()
    {
        BuildInventory();
    }

    public void BuildInventory()
    {
        ClearInventory();

        // --- External data override ---
        List<InventoryItemEntry> activeItemList = itemEntries;

        if (useExternalDataSource && externalData != null)
        {
            activeItemList = externalData.itemEntries;
        }

        // --- Step 1: Compile flat list of item prefabs based on quantity ---
        List<GameObject> itemsToPlace = new List<GameObject>();

        foreach (var entry in activeItemList)
        {
            for (int i = 0; i < entry.quantity; i++)
                itemsToPlace.Add(entry.itemPrefab);
        }

        // Add empty slots
        for (int i = 0; i < emptySlotCount; i++)
            itemsToPlace.Add(null);

        // --- Step 2: Order or Shuffle ---
        if (randomPlacement)
        {
            Shuffle(itemsToPlace);
        }

        // --- Step 3: Fill slots ---
        int count = Mathf.Min(itemsToPlace.Count, itemSlots.Count);

        for (int i = 0; i < count; i++)
        {
            if (itemsToPlace[i] != null)
            {
                GameObject newItem = Instantiate(itemsToPlace[i], itemSlots[i].transform);
                generatedItems.Add(newItem);
            }
        }
    }

    private void ClearInventory()
    {
        foreach (var slot in itemSlots)
        {
            for (int i = slot.transform.childCount - 1; i >= 0; i--) // make sure all slots are filled or it will break here
            {
                Destroy(slot.transform.GetChild(i).gameObject);
                
            }
        }
        generatedItems.Clear();
    }

    private void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            GameObject temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}

public class InventoryExternalData
{
    public List<InventoryItemEntry> itemEntries;
}
