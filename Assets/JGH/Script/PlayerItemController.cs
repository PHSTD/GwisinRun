using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemController : MonoBehaviour, IInteractable
{
    // 슬롯 6개
    [SerializeField]
    private GameObject[] m_itemSlots = new GameObject[6];

    // 현재 장착된 아이템
    [SerializeField]
    private GameObject m_equippedItem;

    [SerializeField]
    private Collider m_riggerItem;

    void Update()
    {
        if (m_riggerItem != null && GameManager.Instance.Input.InteractionKeyPressed)
        {
            bool result = StoreItem(m_riggerItem.gameObject);

            if (result)
            {
                m_riggerItem = null;
            }
        }
        
        // 슬롯 번호(1~6) 키 입력 체크하여 장착
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
            {
                EquipItem(i);
            }
        }

        // 버리기 (예: Backspace 키)
        // TODO: 버리기 버튼 인터페이스 변경 필요
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DropItem();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Items"))
        {
            m_riggerItem = other;
        }
        else
        {
            m_riggerItem = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_riggerItem = null;
    }

    // 아이템을 슬롯에 저장 (외부에서 호출, 예: 아이템 상호작용시)
    public bool StoreItem(GameObject item)
    {
        for (int i = 0; i < m_itemSlots.Length; i++)
        {
            if (m_itemSlots[i] == null)
            {
                m_itemSlots[i] = item;
                item.SetActive(false); // 씬에서 숨김
                Debug.Log($"{i+1}번 슬롯에 {item.name} 저장!");
                return true;
            }
        }
        Debug.Log("슬롯이 모두 찼음!");
        return false;
    }

    // 슬롯의 아이템을 장착
    public void EquipItem(int slotIndex)
    {
        if (m_itemSlots[slotIndex] != null)
        {
            m_equippedItem = m_itemSlots[slotIndex];
            Debug.Log($"{slotIndex+1}번 슬롯의 {m_equippedItem.name} 장착!");
            // 실제 장착 로직 구현 (ex: 손에 들기, 활성화 등)
        }
        else
        {
            Debug.Log("해당 슬롯에 아이템이 없음");
        }
    }

    // 장착된 아이템을 버림
    public void DropItem()
    {
        if (m_equippedItem != null)
        {
            m_equippedItem.SetActive(true); // 씬에 다시 보이게
            m_equippedItem.transform.position = this.transform.position + this.transform.forward * 2; // 앞에 떨어뜨림
            Debug.Log($"{m_equippedItem.name}을(를) 버림!");

            // 슬롯에서 제거
            for (int i = 0; i < m_itemSlots.Length; i++)
            {
                if (m_itemSlots[i] == m_equippedItem)
                {
                    m_itemSlots[i] = null;
                    break;
                }
            }
            m_equippedItem = null;
        }
        else
        {
            Debug.Log("버릴 아이템이 없음");
        }
    }

    public void Interact()
    {
    }
}
