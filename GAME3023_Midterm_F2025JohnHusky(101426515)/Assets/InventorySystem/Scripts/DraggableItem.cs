using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// self explanatory
// the core of the draggable item system
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image image;

    [Header("Item Info")]
    public int count = 0;

    public Sprite icon;
    public string description = "";
    public bool isConsumable = false;

    public int width = 1;
    public int height = 1;
    public string itemName = "";

    [Header("Icon Target (optional)")]
    [SerializeField] private Image iconTarget;
    [SerializeField] private TextMeshProUGUI itemCountText;

    [Header("Item Name & Description UI")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    [Header("Drag & Use Keys")]
    public KeyCode dragKey = KeyCode.Mouse0;
    public KeyCode useKey = KeyCode.Mouse1;

    [Header("Swap Settings")]
    public bool allowItemSwap = true;  // <-- NEW TOGGLE

    [HideInInspector] public Transform parentAfterDrag;

    private bool isDragging = false;
    private Transform originalSlot; // <-- store the original slot

    private void Start()
    {
        UpdateCountText();
        ApplyIcon(icon);
    }

    private void UpdateCountText()
    {
        if (itemCountText == null) return;
        itemCountText.text = count > 0 ? count.ToString() : "0";
    }

    private void ApplyIcon(Sprite s)
    {
        if (s == null) return;
        var target = iconTarget != null ? iconTarget : image;
        if (target != null) target.sprite = s;
    }

    public void newTEXT()
    {
        if (itemNameText != null) itemNameText.text = itemName;
        if (itemDescriptionText != null) itemDescriptionText.text = description;
    }

    // ─────────────────────────────────────────────

    public void OnBeginDrag(PointerEventData eventData)
    {
        newTEXT();

        isDragging = true;
        originalSlot = transform.parent; // <-- remember original slot
        parentAfterDrag = transform.parent;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        if (image != null) image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        newTEXT();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check the object under the cursor
        GameObject hoveredObj = eventData.pointerCurrentRaycast.gameObject;

        // no valid slot hit → return to original slot
        if (hoveredObj == null)
        {
            ReturnToOriginalSlot();
            return;
        }

        // Check if it's a valid slot
        Transform targetSlot = hoveredObj.GetComponent<DraggableItem>() == null
            ? hoveredObj.transform // just a slot (empty)
            : hoveredObj.transform.parent; // hit another item, get its parent slot

        DraggableItem otherItem = targetSlot.GetComponentInChildren<DraggableItem>();

        // CASE 1: Slot occupied & swap allowed
        if (otherItem != null && otherItem != this && allowItemSwap)
        {
            SwapItems(otherItem);
        }
        else
        {
            // CASE 2: Slot empty OR swap disabled → default behavior
            transform.SetParent(targetSlot);
        }

        transform.localPosition = Vector3.zero;

        if (image != null) image.raycastTarget = true;
        isDragging = false;
    }

    // ─────────────────────────────────────────────
    // RETURN TO ORIGINAL SLOT
    private void ReturnToOriginalSlot()
    {
        transform.SetParent(originalSlot);
        transform.localPosition = Vector3.zero;

        if (image != null) image.raycastTarget = true;
        isDragging = false;
    }

    // ─────────────────────────────────────────────
    // SWAP ITEMS FUNCTION
    private void SwapItems(DraggableItem other)
    {
        Transform otherSlot = other.transform.parent;

        // move other item to your old slot
        other.transform.SetParent(originalSlot);
        other.transform.localPosition = Vector3.zero;

        // move YOU to their old slot
        transform.SetParent(otherSlot);
        transform.localPosition = Vector3.zero;
    }

    // ─────────────────────────────────────────────

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging) return;

        if (eventData.button != PointerEventData.InputButton.Left) return;

        newTEXT();

        count = Mathf.Max(0, count - 1);

        if (count <= 0)
        {
            if (itemNameText != null) itemNameText.text = "";
            if (itemDescriptionText != null) itemDescriptionText.text = "";

            Destroy(gameObject);
            return;
        }

        UpdateCountText();
    }
}
