using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;
    private Action<Item> useItemAction;

    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();

        AddItem(new Item {itemType = Item.ItemType.HealthPotion, amount = 0});
        AddItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 0});
     
        Debug.Log("Inventory created");
        Debug.Log("Item count is " + itemList.Count);
    }
    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach(Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory) 
            {
                itemList.Add(item);
            }
        } else
        {
            itemList.Add(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if(inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if(itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UseItem(Item item)
    {
        useItemAction(item);        
    }

    public List<Item> GetItemList() 
    {
        return itemList;
    }

    public void Clear()
    {
        itemList.Clear();
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetItemIndex(Item.ItemType itemType)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemType == itemType)
            {
                return i;
            }
        }
        return -1; // Return -1 if the item is not found
    }
}
