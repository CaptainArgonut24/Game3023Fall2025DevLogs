# 🎒 Grid-Based Inventory System Package  
**GAME3023 – Midterm Exam (Fall 2025)**  
**Author:** John Husky  
**Student ID:** 101426515  
**Date:** November 14, 2025  
---
This project was built from the **provided starter project**, maintaining its art style and UI structure while extending its functionality and organization.

---

## 🧩 Overview
This Unity package implements a **grid-based inventory system** template for unity 2d. can be used for 3d, but this was built for 2d. this dev pack can allow the deive esily t o make a item inviory system that you can drag and drop into mutle slots.
Items have shapes and sizes that occupy grid cells within **inventory containers** (such as a backpack, chest, or shop or whatevr).  
Players can **drag and drop items**, and developers can easily create or customize new items and containers without modifying core code.
eary for the player, easy for teh dev



---

## 🎮 Player Features
- 🧱 **Grid-Based Slots:** Each item has defined dimensions (e.g., 1x1, 2x2) that the dev can set.
- items can be swaped locations, depending on the devs settins,
- 🖱️ **Drag & Drop Support:** Move items within a container or between containers using mouse drag.
- 🚫 **Collision Checking:** Items cannot overlap or be placed outside the container bounds.


---

## 🧰 Developer Features (Back-End)
- ⚙️ **Scriptable Item and prefabs templates Data:** Each item is defined as a ScriptableObject/ prefab containing all the data needed to run.
- 📦 **Modular ui set up:** Create different sections, pannales (e.g., Player Backpack, Chest, Shop) and set their sizes using customizable grid dimensions.
- 🧩 **Generic Grid System:** Supports different item sizes and grid configurations.
- 🔧 **Easy Setup:** all the Developers need to do is assign the slots to tehir likeing, items, datastore, ect. all this does it shows where teh diretory is and disblase it in real time for teh player. 
---
how it works

the invatiry grid works by the dev putting all avilves slots into a list, then takeing all the items, and how many times tey want to apreir on the grid. then when it loeds all the items takeingf orm the list will show up in oder or randoly on the grid alowung the users to mobve and swap places iwth other items on the grid without it leaveing the grid.

---
how to set up
all the suff you'll need is in the "InventorySystem" folder insded of assests, scriots,m prefabs, scrible objects, spriets ect. 

- make an item: either with the Pefab (ItemPrefab.BASE) or the Scriptable Objects (Scriptable Objects.TEST) set all the info you want, scuh as, name, discriptoio, ammount ect
- InventoryHandleler: This script builds an inventory UI by taking a list of item slots and filling them with item prefabs according to how many copies you configure. When the game starts, it runs BuildInventory(), which first clears all old items from the slots, then chooses which item list to use (either the default list or an external data source if enabled). It expands those entries into a flat list based on their quantities, optionally adds empty spaces, and then either keeps the order or shuffles it. Finally, it loops through the UI slots and instantiates each item prefab into the corresponding slot. The script basically automates generating an inventory layout when the game starts, with optional randomization and external data support.
- DraggableItem: This script controls how an inventory item behaves when clicked, dragged, or swapped. It tracks item data like name, description, icon, and quantity, updates the UI text, and remembers the original slot when dragging begins. While dragging, the item follows the mouse and temporarily moves to the top of the UI. When released, the script checks what the mouse is over: if it's an empty slot, the item snaps there; if it's another item and swapping is allowed, the two items switch places; otherwise it returns to its original slot. Clicking the item (without dragging) consumes one quantity and updates the text, destroying the item if its count reaches zero.
- InventorySlot: This script controls what happens when a draggable item is dropped onto an inventory slot. It implements IDropHandler, so Unity calls OnDrop() when the mouse releases an item over this slot. If the slot is empty (has no children), it takes the dragged object and updates that item’s parentAfterDrag so the DraggableItem script knows this slot is its new home. The slot doesn’t move the item itself—it simply tells the dragged item where it should be placed after the drag ends.




---
known bubgs

- item name and discuprion donst dupate constlinyt
- TMP is buggy, might be my PCs
- pull from user data is hit or miss
- mutples slots brik the game. so its commented out.
- assing can be a bit janky.
- 



**References:**  
- Unity Manual: [for refrince help](https://docs.unity3d.com/Manual/UISystem.html)  
- Drag and drop base and ideas:  https://www.youtube.com/watch?v=kWRyZ3hb1Vc, https://www.youtube.com/watch?v=oJAE6CbsQQA&t, https://www.youtube.com/watch?v=rWMvQwwGOtg&t, https://www.youtube.com/playlist?list=PLSR2vNOypvs6eIxvTu-rYjw2Eyw57nZrU
- all asets were from the proveded starter pprohect
-  Scriptable Objects(want used much):  https://www.youtube.com/watch?v=tuc_6ooZE1Q
-  chat bot and coplie for clean up and spile error fixing (eg, mispladed formating or values)
-  looking at how othe people ask the same question: https://www.reddit.com/r/unity/comments/ykyout/best_way_to_create_an_inventory_system/
-  some otehr helpful tudtoresas and links used for ideas and help: https://www.youtube.com/watch?v=SGz3sbZkfkg, [https://www.youtube.com/watch?v=SGz3sbZkfkg],(https://www.youtube.com/watch?v=-IPjFSWeyrg), https://jaredamlin.medium.com/setting-up-an-inventory-system-with-scriptable-objects-in-unity-176599ca49bb, https://discussions.unity.com/t/how-to-script-an-inventory-system/922301, https://gamedev.stackexchange.com/questions/211432/how-to-structure-a-complex-inventory-system-in-unity, https://www.youtube.com/playlist?list=PLcRSafycjWFegXSGBBf4fqIKWkHDw_G8D. 
---

compaion video: 
