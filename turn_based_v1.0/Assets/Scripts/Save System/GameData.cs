using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public string currentScene;
    public List<HeroData> heroesData;
    public InventoryData mageInventory;
    public List<string> defeatedSpawners;
    public List<string> pickedItems;
}

[Serializable]
public class HeroData
{
    public string name;
    public int health;
    public int mana;
    public float positionX; // Store x component of the position
    public float positionY; // Store y component of the position
}

[Serializable]
public class InventoryData
{
    public List<Item> items;
}