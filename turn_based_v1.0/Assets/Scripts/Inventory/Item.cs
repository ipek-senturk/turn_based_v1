using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Item
{
    public enum ItemType
    {
        HealthPotion,
        ManaPotion
    }
    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion: return ItemAssets.Instance.hpSprite;
            case ItemType.ManaPotion:   return ItemAssets.Instance.mpSprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
        default:
        case ItemType.HealthPotion:
        case ItemType.ManaPotion:
        return true;
        }
    }
}
