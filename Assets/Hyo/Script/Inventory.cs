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
    
    //# 수정 사항(20250503) -- 시작
    private int m_selectedItemIndex;
    public int SelectedItemIndex => m_selectedItemIndex;

    void Update()
    {
        if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
            return;
        
        if (GameManager.Instance.Input.ItemsActionPressed)
        {
            m_selectedItemIndex = GameManager.Instance.Input.LastPressedKey;
            Debug.Log($"{m_selectedItemIndex+ 1} 번째 슬롯 선택");
        }
    }
    //# 수정 사항(20250503) -- 끝

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

    //# 수정 사항(20250503) -- 시작
    public void UseItem()
    {
        if (m_selectedItemIndex < 0 || m_selectedItemIndex >= items.Count)
        {
            Debug.Log("해당 슬롯에 아이템이 없습니다.");
            return;
        }

        if (items[m_selectedItemIndex] is IUsable)
        {
            UsableItem item = items[m_selectedItemIndex] as UsableItem;
            if (item == null)
                return;
            
            OnUseItem?.Invoke(item.ItemName, item.Value); 
            items.RemoveAt(m_selectedItemIndex);
            item.Use();
            UpdateInventoryUI();
            Debug.Log($"{m_selectedItemIndex + 1} 슬롯에 아이템을 사용합니다.");
        }
    }
    //# 수정 사항(20250503) -- 끝
}
