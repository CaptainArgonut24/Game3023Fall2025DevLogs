using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [System.Serializable]
    public class ItemEntry
    {
        public string itemName;
        public int amount;
        public Sprite image;
        [Range(0f, 1f)] public float chance = 1f; // 0–1 chance value
    }

    [Header("Item Settings")]
    public List<ItemEntry> items = new List<ItemEntry>();

    [Header("Respawn Settings")]
    public float respawnTime = 5f;

    private SpriteRenderer spriteRenderer;
    private ItemEntry currentItem;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        PickRandomItem();
    }

    void PickRandomItem()
    {
        float totalWeight = 0f;

        foreach (var item in items)
            totalWeight += item.chance;

        float randomValue = Random.value * totalWeight;
        float current = 0f;

        foreach (var item in items)
        {
            current += item.chance;
            if (randomValue <= current)
            {
                currentItem = item;
                break;
            }
        }

        spriteRenderer.sprite = currentItem.image;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        GameData data = collision.GetComponent<GameData>();
        if (data == null)
        {
            Debug.LogWarning("Player is missing GameData.cs!");
            return;
        }

        data.AddItem(currentItem.itemName, currentItem.amount);

        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(respawnTime);

        PickRandomItem();
        spriteRenderer.enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
}
