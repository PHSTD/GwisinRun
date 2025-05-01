using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlotData
{
    public string ItemName;
    public ItemType Type;
    public Dictionary<string, int> Values;    // 예: 방어력, 마법 저항 등
    public GameObject ItemObject;             // 실제 아이템 오브젝트
}
