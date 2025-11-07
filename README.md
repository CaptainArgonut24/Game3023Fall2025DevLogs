# 🎒 Grid-Based Inventory System Package  
**GAME3023 – Midterm Exam (Fall 2025)**  
**Author:** John Husky  
**Student ID:** 101426515  
**Date:** November 14, 2025  

---

## 🧩 Overview
This Unity package implements a **grid-based inventory system** similar to games such as *Resident Evil*, *Diablo*, and *Escape from Tarkov*.  
Items have shapes and sizes that occupy grid cells within **inventory containers** (such as a backpack, chest, or shop).  
Players can **drag and drop items**, and developers can easily create or customize new items and containers without modifying core code.

This project was built from the **provided starter project**, maintaining its art style and UI structure while extending its functionality and organization.

---

## 🎮 Player Features (Front-End)
- 🧱 **Grid-Based Slots:** Each item has defined dimensions (e.g., 1x1, 2x3) and occupies multiple grid spaces.
- 🖱️ **Drag & Drop Support:** Move items within a container or between containers using mouse drag.
- 🚫 **Collision Checking:** Items cannot overlap or be placed outside the container bounds.
- ✨ **Visual Feedback:** Invalid placement zones are highlighted.
- 🎨 **Simple & Clean UI:** Maintains the look and feel of the provided project.

---

## 🧰 Developer Features (Back-End)
- ⚙️ **Scriptable Item Data:** Each item is defined as a ScriptableObject containing its name, size, and sprite.
- 📦 **Modular Containers:** Create different containers (e.g., Player Backpack, Chest, Shop) using customizable grid dimensions.
- 🧩 **Generic Grid System:** Supports different item sizes and grid configurations.
- 🪶 **Decoupled Architecture:** Uses UnityEvents to handle interactions between containers and items, minimizing code coupling.
- 🔧 **Easy Setup:** Developers can add new items or containers directly in the Unity Editor without writing additional code.

---

## 🗂️ Project Structure
Assets/
├── InventorySystem/
│ ├── Scripts/
│ │ ├── InventoryItem.cs
│ │ ├── GridContainer.cs
│ │ ├── InventoryManager.cs
│ │ ├── ItemData.cs (ScriptableObject)
│ │ ├── DragDropHandler.cs
│ │ └── UIGridSlot.cs
│ ├── Prefabs/
│ │ ├── Item.prefab
│ │ ├── Container.prefab
│ │ └── ExampleItemSet.prefab
│ ├── Demo/
│ │ ├── DemoScene.unity
│ │ ├── DemoUI.prefab
│ │ └── ExampleItems/
│ └── README.md
└── ...
All inventory-related files are contained within the **InventorySystem** folder to make the package **portable**.

---

## 🚀 How to Use
### 🧱 For Players
1. Run the **Demo Scene** located at  
   `Assets/InventorySystem/Demo/DemoScene.unity`
2. Use the mouse to:
   - Drag items into empty slots.
   - Move items between containers.
   - Try invalid placements (to see collision prevention in action).

### 🧑‍💻 For Developers
1. Open the `InventorySystem` folder in Unity.
2. To create a new **Item**:
   - Right-click → *Create → Inventory → Item Data*
   - Set its name, icon, width, and height.
3. To create a new **Container**:
   - Duplicate an existing container prefab.
   - Adjust its grid dimensions in the Inspector.
4. Assign items to containers using the Inspector or runtime setup scripts.

---

## 💡 Design Decisions
- ✅ **Rectangle-Only Shapes:** For simplicity and better usability, items are limited to rectangular shapes (e.g., 1x1, 2x2, 2x3).
- ✅ **Grid Validation:** The grid checks all cells an item would occupy before allowing placement.
- ✅ **Event-Driven Architecture:** Used UnityEvents to keep item-container communication modular and flexible.
- ✅ **Expandable Package:** Can be easily reused in other games with different art or UI layouts.

---

## 🧾 Documentation & Video
- 🎥 **Video Presentation (YouTube - Unlisted):** [INSERT LINK HERE]  
  *(Demonstrates functionality, setup process, and design rationale.)*
- 📘 **Source Code (GitHub Commit):** [INSERT COMMIT LINK HERE]  
  *(Commit includes full Unity project and this README.)*

---

## 🧠 Future Improvements
- 🔄 Item rotation support (e.g., rotate 2x3 → 3x2).
- 🧩 Irregular item shapes (L/T/S patterns).
- 🪣 Item stacking for consumables.
- 🧍 Equipment slot integration (armor, weapons, etc.).
- 💾 Save/Load inventory state to file or player prefs.

---

## ⚖️ Academic Integrity
All scripts and assets were **created by me (John Husky)** specifically for this midterm exam.  
No external code or artwork was imported.  
Any tutorial or reference material used for learning purposes has been cited below.

**References:**  
- Unity Manual: [UI Toolkit & EventSystem Docs](https://docs.unity3d.com/Manual/UISystem.html)  
- Unity Learn: [Drag and Drop with UI](https://learn.unity.com/tutorial/ui-drag-and-drop)  
- [Personal research and experimentation]

---

## 📦 Version Info
- Unity Version: **2022.3 LTS**
- Target Platform: **Windows / PC**
- Package Type: **Standalone Add-On**

---

© 2025 John Husky – GAME3023 Midterm Project


