# 🌱 Unity Garden Project – Final Exam

This project is a **2D garden simulation game** developed using **Unity** for a **final exam submission**.

The game demonstrates player interaction, AI behavior, crop harvesting, UI systems, and data persistence using JSON.

---

## 🎮 Gameplay Overview

The player can:
- Move freely around the garden
- Attack skeleton enemies
- Interact with animals and goblins
- Dig and harvest crops
- Collect items and track them in an inventory UI
- Pause the game and view tutorial controls

---

## 🧑‍🌾 Player Controls

| Action | Input |
|------|------|
| Move | WASD |
| Attack | Mouse Click or Space |
| Dig | `E` |
| Pause / Tutorial | `ESC` or `?` button |

---

## 👾 Features

### Skeleton Enemy
- Chases and attacks the player
- Has HP, hurt, death, and respawn system
- Respawns after a delay at its original position

### Animals
- Idle movement within a small area
- Plays sound when player is nearby
- Shows **Heart emote** when approached
- Shows **Heartbreak emote** when attacked

### Goblin NPC
- Friendly NPC with random movement
- Plays different voice reactions based on player interaction
- Reacts when nearby or attacked

---

## 🌾 Crop System

- Minimum **3 crop types** (Carrot, Cabbage, Beetroot)
- Crops remain planted until dug by the player
- Digging spawns collectible crop items
- Collected crops disappear and respawn after **10 seconds**

---

## 📦 Inventory & Save System

- Separate counter for each crop type
- Inventory displayed on the top-left UI
- Data is saved automatically using a **JSON file**
- Inventory is loaded when the game starts
- Includes **Reset Save** option in the Welcome UI

---

## 🖥️ User Interface

- **Welcome / Tutorial Panel**
  - Displays controls, developer info, and start button
  - Pauses the game when active
- **Tutorial Button (`?`)**
  - Opens the tutorial panel during gameplay
- **Collectible UI**
  - Shows collected crop values
  - Hidden when the game is paused

---

## 🛠️ Technologies Used

- Unity
- C#
- TextMeshPro
- 2D Physics & Animation
- JSON Data Persistence

---

## 👩‍💻 Author

**Emily Wilkinson**  
Unity Garden Project – Final Exam
