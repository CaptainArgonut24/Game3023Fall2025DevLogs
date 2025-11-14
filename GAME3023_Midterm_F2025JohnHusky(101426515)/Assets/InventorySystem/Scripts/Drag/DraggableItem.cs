using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image image;

    [Header("Item Info")]
    // data lives on this component now
    public int count = 0;

    // local item data (used instead of Item scriptable object)
    public Sprite icon;
    public string description = "";
    public bool isConsumable = false;

    public int width = 1;
    public int height = 1;
    public string itemName = "";

    // Optional: separate Image target for the item icon in the UI.
    // If null, `image` will be used as the icon target.
    [Header("Icon Target (optional)")]
    [SerializeField] private Image iconTarget;

    [SerializeField] private TextMeshProUGUI itemCountText;

    [Header("Item Name & Description UI")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;


    [Header("Drag & Use Keys")]
    public KeyCode dragKey = KeyCode.Mouse0;
    public KeyCode useKey = KeyCode.Mouse1;

    [HideInInspector] public Transform parentAfterDrag;

    private bool isDragging = false;

    private void Start()
    {
        UpdateCountText();
        // Ensure UI reflects local data at start
        // Apply icon on load to configured target
        ApplyIcon(icon);
    }

    private void UpdateCountText()
    {
        if (itemCountText == null) return;
        itemCountText.text = count > 0 ? count.ToString() : "0";
    }

    // Apply sprite to the icon target (iconTarget if assigned, otherwise fallback to image)
    private void ApplyIcon(Sprite s)
    {
        if (s == null) return;
        var target = iconTarget != null ? iconTarget : image;
        if (target != null) target.sprite = s;
    }

    // Public so other systems can request the item UI be refreshed
    public void newTEXT()
    {
        // Update item name and description UI
        if (itemNameText != null) itemNameText.text = itemName;
        if (itemDescriptionText != null) itemDescriptionText.text = description;
    }
public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");

        // update UI from local fields when dragging starts
        newTEXT();

        isDragging = true;

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        if (image != null) image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
        newTEXT();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag");

        isDragging = false;

        transform.SetParent(parentAfterDrag);
        if (image != null) image.raycastTarget = true;
    }

    // Handle clicks to consume the item and update UI
    public void OnPointerClick(PointerEventData eventData)
    {
        // Ignore clicks that happen while dragging
        if (isDragging) return;

        // Only respond to left-click
        if (eventData.button != PointerEventData.InputButton.Left) return;

        // FIRST update the UI
        newTEXT();

        // Subtract one and update UI/remove if zero
        count = Mathf.Max(0, count - 1);

        if (count <= 0)
        {
            // Clear UI so it does not show deleted item
            if (itemNameText != null) itemNameText.text = "";
            if (itemDescriptionText != null) itemDescriptionText.text = "";

            Destroy(gameObject);
            return;
        }

        UpdateCountText();
        Debug.Log("Updated UI for item: " + itemName);
    }

}