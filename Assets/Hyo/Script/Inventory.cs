using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public int maxSlots = 6;
    public Item[] items;
    private int m_itemCount;
    
    
    public UnityEvent<string, int> OnUseItem;
    public UnityEvent<int> OnDropOrUseItem;
    public UnityEvent<int, Item> OnAddItem;
    public UnityEvent<int> OnSelectedItemChanged;
    
    //# 수정 사항(20250503) -- 시작
    private int m_selectedItemIndex;
    public int SelectedItemIndex => m_selectedItemIndex;

    private void Awake()
    {
        items = new Item[maxSlots];
    }

    void Update()
    {
        if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
            return;
        
        if (GameManager.Instance.Input.ItemsActionPressed)
        {
            m_selectedItemIndex = GameManager.Instance.Input.LastPressedKey;
            // Debug.Log($"{m_selectedItemIndex+ 1} 번째 슬롯 선택");
            OnSelectedItemChanged?.Invoke(m_selectedItemIndex);
        }
    }
    //# 수정 사항(20250503) -- 끝

    public bool AddItem(Item item)
    {
        if (m_itemCount >= maxSlots)
        {
            Debug.Log("인벤토리가 가득 찼습니다.");
            return false;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
                continue;
            
            items[i] = item;
            item.gameObject.SetActive(false);
            OnAddItem?.Invoke(i, items[i]);
            
            Debug.Log($"{item.ItemName} 아이템을 획득했습니다.");
            m_itemCount++;
            
            break;
        }
        
        return true;
    }

    //# 수정 사항(20250503) -- 시작
    public void UseItem()
    {
        if (m_selectedItemIndex < 0 || m_selectedItemIndex >= items.Length)
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
            OnDropOrUseItem?.Invoke(m_selectedItemIndex);
            
            items[m_selectedItemIndex] = null;
            item.Use();
            m_itemCount--;
            Debug.Log($"{m_selectedItemIndex + 1} 슬롯에 아이템을 사용합니다.");
        }
    }

    public bool RemoveKey()
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].ItemName == "Key")
            {
                OnDropOrUseItem?.Invoke(i);
                items[i] = null;
                Destroy(items[i].gameObject);
                m_itemCount--;
                return true;
            }
        }
        return false;
    }
}
