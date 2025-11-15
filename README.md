# 🎒 Grid-Based Inventory System  
**GAME3023 – Midterm Exam (Fall 2025)**  
**Author:** John Husky  
**Student ID:** 101426515  
**Date:** November 14, 2025  

This project is built using the **provided starter project**, keeping its original UI style and art direction while expanding the functionality, structure, and developer workflow.

---

## 🧩 Overview

This Unity package implements a **grid-based inventory system** designed for **2D games**, with optional support for 3D setups.  
It provides an easy plug-and-play structure where developers can create item inventories with drag-and-drop interaction across multiple slot grids.

Items have customizable shapes and sizes and occupy grid cells inside **inventory containers** such as backpacks, chests, and shops.  
Players can freely **drag, drop, move, and (optionally) swap items**, while developers can create new items or inventory panels without modifying core code.

---

## 🎮 Player Features

- 🧱 **Grid-Based Item Placement**  
  Each item has developer-defined dimensions (1×1, 2×2, etc.).

- 🔄 **Optional Item Swapping**  
  Items can swap positions if swapping is enabled in the settings.

- 🖱️ **Drag & Drop Interaction**  
  Items can be moved within or between inventory containers.

- 🚫 **Collision & Boundary Checking**  
  Items cannot overlap or exceed container bounds.

---

## 🧰 Developer Features

- ⚙️ **ScriptableObject / Prefab Item Templates**  
  Items store all required data (name, description, icon, size, amount, etc.).

- 🧩 **Modular UI Setup**  
  Build multiple inventory sections (Backpack, Chest, Shop, etc.) with customizable grid dimensions.

- 📦 **Generic Grid System**  
  Supports any number of rows/columns and any item size.

- 🔧 **Fast & Simple Setup**  
  Developers only assign slot objects, item definitions, and optional data sources.  
  The system automatically generates the final UI at runtime.

---

## ⚙️ How the System Works

1. The developer assigns all inventory slot GameObjects into a list.  
2. The developer assigns item definitions and specifies how many of each item should appear.  
3. The system expands those items into a full list (based on quantity).  
4. It optionally randomizes the order.  
5. The grid is populated with one item per slot on load.  
6. The player can then move or swap items—items never leave the grid unless consumed or removed.

---

## 🛠️ How to Set Up

All tools and assets are located in the **InventorySystem** folder:

- Scripts  
- Prefabs  
- ScriptableObjects  
- Sprites  
- Example scenes  

### ✔ Creating an Item

You can make an item using either:

- **Prefab Template:** `ItemPrefab.BASE`  
- **ScriptableObject Template:** `ScriptableObjects.TEST`

Configure fields such as:

- Item Name  
- Description  
- Icon  
- Quantity  
- Dimensions  
- Any custom data your project needs  

---

## 📜 Script Breakdown

### **InventoryHandler**
- Builds the UI inventory by taking a list of slots and filling them with item prefabs.
- Generates a complete list of items based on quantity.
- Can randomize item order.
- Supports using default or external data sources.
- Automatically clears and repopulates the inventory on load.

> In short: **It auto-generates the entire inventory layout at runtime.**

---

### **DraggableItem**
Handles all player interaction:

- Stores item data (name, icon, description, quantity).  
- Updates the UI text dynamically.  
- Saves its original slot when dragging begins.  
- Follows the mouse and moves to the top of the UI hierarchy while dragging.  
- On drop:
  - Empty slot → item moves there  
  - Occupied slot (swap enabled) → items swap  
  - Invalid spot → returns to original slot  
- Clicking (without dragging) consumes one item and destroys the object when quantity reaches 0.

---

### **InventorySlot**
- Implements **IDropHandler** to detect item drops.  
- Accepts an item only if the slot is empty.  
- Updates the item’s `parentAfterDrag` reference, allowing DraggableItem to finalize placement.

---

## 🐞 Known Issues

- Item name and description don’t update consistently.  
- TextMeshPro can be unreliable depending on the machine.  
- User data loading (save data) may fail intermittently.  
- Multiple inventory panels at once may break layout logic.  
- Slot assignment can sometimes feel inconsistent.

---

## 📚 References & Resources

- Unity UI Manual  
  https://docs.unity3d.com/Manual/UISystem.html

- Drag & Drop Tutorials  
  https://www.youtube.com/watch?v=kWRyZ3hb1Vc  
  https://www.youtube.com/watch?v=oJAE6CbsQQA  
  https://www.youtube.com/watch?v=rWMvQwwGOtg  
  https://www.youtube.com/playlist?list=PLSR2vNOypvs6eIxvTu-rYjw2Eyw57nZrU

- ScriptableObject Tutorial  
  https://www.youtube.com/watch?v=tuc_6ooZE1Q

- Additional Reading  
  https://www.reddit.com/r/unity/comments/ykyout/best_way_to_create_an_inventory_system/  
  https://www.youtube.com/watch?v=SGz3sbZkfkg  
  https://www.youtube.com/watch?v=-IPjFSWeyrg  
  https://jaredamlin.medium.com/setting-up-an-inventory-system-with-scriptable-objects-in-unity-176599ca49bb  
  https://discussions.unity.com/t/how-to-script-an-inventory-system/922301  
  https://gamedev.stackexchange.com/questions/211432/how-to-structure-a-complex-inventory-system-in-unity  
  https://www.youtube.com/playlist?list=PLcRSafycjWFegXSGBBf4fqIKWkHDw_G8D  

- Assets used are from the provided course starter project.
- ChatBotGTP, JetBrains and Copilot Used to help with errors and mising links. Also writeing and spelling checks.  

---

## 🎥 Companion Video  
https://youtu.be/xb5cgB7wFFY

---
