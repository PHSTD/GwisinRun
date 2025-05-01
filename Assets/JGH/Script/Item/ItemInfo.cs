using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    // 아이템 속성 구조체 (예: 방어력, 마법 저항)
    [Serializable]
    public struct ItemAttribute
    {
        public string Key;
        public int Value;
    }

    // 아이템 타입 열거형
    public enum ItemType
    {
        Weapon,
        Key,
        Light,
        Potion
    }

    // 슬롯에 저장되는 아이템 데이터 구조체
    [Serializable]
    public struct ItemSlotData
    {
        public string ItemName;
        public ItemType Type;
        public Dictionary<string, int> Values;
        public GameObject ItemObject;
    }
    
    [SerializeField] private string m_itemName;
    [SerializeField] private ItemType m_type;

    public string ItemName => m_itemName;
    public ItemType Type => m_type;

    [SerializeField] private List<ItemAttribute> attributes = new List<ItemAttribute>();
    public List<ItemAttribute> Attributes => attributes;

    public Dictionary<string, int> GetAttributeDictionary()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        foreach (var attr in attributes)
        {
            dict[attr.Key] = attr.Value;
        }
        return dict;
    }

}