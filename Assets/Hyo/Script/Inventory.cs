using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public int maxSlots = 6;
    public List<Item> items = new List<Item>();
    public UnityEvent<string, int> OnUseItem;

    void Update()
    {
        // for (int i = 0; i < 6; i++)
        // {
        //     if (Input.GetKeyDown(KeyCode.Alpha1 + i))
        //     {
        //         Debug.Log($"{i + 1}번 슬롯 아이템 사용 시도");
        //         UseItem(i);
        //     }
        // }
    }

    public bool AddItem(Item item)
    {
        if (items.Count >= maxSlots)
        {
            Debug.Log("인벤토리가 가득 찼습니다.");
            return false;
        }

        items.Add(item);
        item.gameObject.SetActive(false);
        Debug.Log($"{item.ItemName} 아이템을 획득했습니다.");
        UpdateInventoryUI();
        return true;
    }

    void UpdateInventoryUI() // 여기서 UI 구현
    {
        if (items.Count == 0)
            return;
        
        Debug.Log($"인벤토리 상태: {string.Join(", ", items)}");
    }

    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= items.Count)
        {
            Debug.Log("해당 슬롯에 아이템이 없습니다.");
            return;
        }

        if (items[slotIndex] is IUsable)
        {
            UsableItem item = items[slotIndex] as UsableItem;
            if (item == null)
                return;
            
            OnUseItem?.Invoke(item.ItemName, item.Value); 
            items.RemoveAt(slotIndex);
            item.Use();
            UpdateInventoryUI();
        }

    }
}
