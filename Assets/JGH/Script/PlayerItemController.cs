using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour, IInteractable
{
    [Header("아이템 슬롯")]
    [SerializeField]
    private GameObject[] m_itemSlots = new GameObject[6]; // 총 6개의 슬롯

    private int m_equippedSlotIndex = -1;                 // 현재 장착 중인 슬롯 인덱스 (-1: 없음)
    [SerializeField]
    private GameObject m_equippedItem;                    // 현재 장착된 아이템

    private Collider m_riggerItem;                        // 현재 감지된 아이템 콜라이더

    void Update()
    {
        DetectItemByRay(); // 아이템 탐지 (Ray 방식으로 교체)

        // 상호작용 키로 아이템 저장 시도
        if (m_riggerItem != null && GameManager.Instance.Input.InteractionKeyPressed)
        {
            if (StoreItem(m_riggerItem.gameObject))
            {
                m_riggerItem = null; // 성공 시 감지 초기화
            }
        }

        // 1~6번 키 입력으로 장착 시도
        for (int i = 0; i < GameManager.Instance.Input.ItemKeyPressed.Length; i++)
        {
            if (GameManager.Instance.Input.ItemKeyPressed[i])
            {
                EquipItem(i);
                GameManager.Instance.Input.ItemKeyPressed[i] = false; // 키 입력 초기화
            }
        }

        // 버리기 키 입력
        if (GameManager.Instance.Input.DropKeyPressed)
        {
            DropItem();
        }
    }

    // Ray + SphereCast 방식으로 정면 아래 아이템 감지
    void DetectItemByRay()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 rayDirection = transform.forward;

        if (Physics.SphereCast(rayOrigin, 0.4f, rayDirection, out RaycastHit hit, 1.5f))
        {
            if (hit.collider.CompareTag("Item"))
            {
                m_riggerItem = hit.collider;
                return;
            }
        }

        // 감지 실패 시 초기화
        m_riggerItem = null;
    }

    // 아이템을 슬롯에 저장
    public bool StoreItem(GameObject item)
    {
        for (int i = 0; i < m_itemSlots.Length; i++)
        {
            if (m_itemSlots[i] == null)
            {
                // 같은 이름의 아이템이 장착되어 있으면 해제
                if (m_equippedItem != null && m_equippedItem.name == item.name)
                {
                    UnequipItem();
                }

                m_itemSlots[i] = item;
                item.SetActive(false); // 씬에서 숨김
                m_itemSlots[i].name = m_itemSlots[i].name.Replace("(Clone)", "").Trim(); // 이름 정리

                Debug.Log($"{i + 1}번 슬롯에 {item.name} 저장!");
                return true;
            }
        }

        Debug.Log("슬롯이 모두 찼음!");
        return false;
    }

    // 슬롯 인덱스로 아이템 장착
    public void EquipItem(int slotIndex)
    {
        if (m_itemSlots[slotIndex] == null)
        {
            Debug.Log($"{slotIndex + 1}번 슬롯에 아이템이 없음");
            return;
        }

        // 같은 슬롯을 다시 누른 경우: 해제만
        if (m_equippedSlotIndex == slotIndex)
        {
            UnequipItem();
            return;
        }

        UnequipItem(); // 기존 장착 해제

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다!");
            return;
        }

        // 아이템 인스턴스 생성 및 장착
        m_equippedItem = Instantiate(m_itemSlots[slotIndex], player.transform);
        m_equippedItem.SetActive(true);
        m_equippedItem.transform.localPosition = Vector3.zero;
        m_equippedItem.transform.localRotation = Quaternion.identity;

        m_equippedSlotIndex = slotIndex;

        Debug.Log($"{slotIndex + 1}번 슬롯의 {m_equippedItem.name} 장착 완료");
    }

    // 현재 장착 아이템 해제
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

    // 현재 장착 아이템을 버림
    public void DropItem()
    {
        if (m_equippedItem == null)
        {
            Debug.Log("버릴 아이템이 없음");
            return;
        }

        m_equippedItem.transform.SetParent(null);
        m_equippedItem.SetActive(true);
        m_equippedItem.transform.position = transform.position + transform.right * 1f;

        // 물리 활성화
        Rigidbody rb = m_equippedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        Debug.Log($"{m_equippedItem.name}을(를) 버림!");

        // 슬롯 비우기
        if (m_equippedSlotIndex != -1)
        {
            m_itemSlots[m_equippedSlotIndex] = null;
            Debug.Log($"{m_equippedSlotIndex + 1}번 슬롯 비웠음");
        }

        // 상태 초기화
        m_equippedItem = null;
        m_equippedSlotIndex = -1;
    }

    // 인터페이스용 (필수 구현, 기능 없음)
    public void Interact() { }
}
