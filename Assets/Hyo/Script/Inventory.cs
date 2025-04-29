using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public int maxSlots = 6;
    public List<string> items = new List<string>();

    public bool AddItem(Item newItem)
    {
        if (items.Count >= maxSlots)
        {
            return false; // 인벤토리 꽉 찼으면 추가 실패
        }

        items.Add(newItem.itemName);
        UpdateInventoryUI();
        return true;
    }

    void UpdateInventoryUI() // 여기에 UI 코드 넣으면 됨
    {
        Debug.Log("인벤토리 상태: " + string.Join(", ", items));
    }

    public void UseItem(int slot)
    {
        if (slot < 0 || slot >= items.Count)
            return;

        Debug.Log("아이템 사용: " + items[slot]);
        // 여기에 실제 아이템 사용 로직 구현
        items.RemoveAt(slot);
        UpdateInventoryUI();
    }

}
