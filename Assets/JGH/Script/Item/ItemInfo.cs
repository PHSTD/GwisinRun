using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private string m_itemName;
    [SerializeField] private ItemType m_type;

    public string ItemName => m_itemName;
    public ItemType Type => m_type;

    // 속성들 (예: 방어력, 마법저항)
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