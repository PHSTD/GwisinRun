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
    
    private int m_equippedSlotIndex = -1; // 현재 장착 중인 슬롯 번호 (없으면 -1)

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
        for (int i = 0; i < GameManager.Instance.Input.ItemKeyPressed.Length; i++)
        {
            if (GameManager.Instance.Input.ItemKeyPressed[i])
            {
                Debug.Log($"{i+1} 아이템 키");
                EquipItem(i);
                GameManager.Instance.Input.ItemKeyPressed[i] = false;
            }
        }

        if (GameManager.Instance.Input.DropKeyPressed)
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
                // 만약 현재 장착 중인 아이템이 이 아이템이면 먼저 해제
                if (m_equippedItem != null && m_equippedItem.name == item.name)
                {
                    UnequipItem();
                }

                m_itemSlots[i] = item;
                item.SetActive(false); // 씬에서 숨김
                m_itemSlots[i].name = m_itemSlots[i].name.Replace("(Clone)", "").Trim();

                Debug.Log($"{i + 1}번 슬롯에 {item.name} 저장!");
                return true;
            }
        }
        Debug.Log("슬롯이 모두 찼음!");
        return false;
    }

    public void EquipItem(int slotIndex)
    {
        if (m_itemSlots[slotIndex] == null)
        {
            Debug.Log($"{slotIndex + 1}번 슬롯에 아이템이 없음");
            return;
        }

        // 같은 슬롯을 다시 누르면 탈착만 하고 끝내기
        if (m_equippedSlotIndex == slotIndex)
        {
            UnequipItem();
            return;
        }
        
        // 다른 슬롯을 누른 경우에는 기존 아이템 해제 후 새로 장착
        UnequipItem();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다!");
            return;
        }

        m_equippedItem = Instantiate(m_itemSlots[slotIndex], player.transform);
        m_equippedItem.SetActive(true);
        m_equippedItem.transform.localPosition = Vector3.zero;
        m_equippedItem.transform.localRotation = Quaternion.identity;

        m_equippedSlotIndex = slotIndex;

        Debug.Log($"{slotIndex + 1}번 슬롯의 {m_equippedItem.name} 장착 완료");
    }
    
    private void UnequipItem()
    {
        if (m_equippedItem != null)
        {
            Destroy(m_equippedItem);
            Debug.Log($"{m_equippedSlotIndex + 1}번 슬롯 아이템 해제 완료");

            m_equippedItem = null;
            m_equippedSlotIndex = -1;
        }
    }

    // 장착된 아이템을 버림
    public void DropItem()
    {
        if (m_equippedItem != null)
        {
            // 부모 끊기
            m_equippedItem.transform.SetParent(null);

            // 활성화
            m_equippedItem.SetActive(true);

            // 위치 이동 (플레이어 오른쪽 1m)
            m_equippedItem.transform.position = this.transform.position + this.transform.right * 1f;

            // Rigidbody 물리 적용
            Rigidbody rb = m_equippedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            Debug.Log($"{m_equippedItem.name}을(를) 버림!");

            // 슬롯에서 정확히 비우기
            if (m_equippedSlotIndex != -1)
            {
                m_itemSlots[m_equippedSlotIndex] = null;
                Debug.Log($"{m_equippedSlotIndex + 1}번 슬롯 비웠음");
            }

            // 장착 초기화
            m_equippedItem = null;
            m_equippedSlotIndex = -1;
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
