using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemType { Key, HeartPotion, SpeedPotion, Repellent }
    public ItemType type;
    public int value = 1;
    public string itemName;

    public Item(string name)
    {
        itemName = name;
    }
    public override string ToString()
    {
        return itemName;
    }
}
