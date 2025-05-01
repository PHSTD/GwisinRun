using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField] private ItemInfo.ItemSlotData[] m_itemSlots = new ItemInfo.ItemSlotData[6];

    private int m_equippedSlotIndex = -1;
    private GameObject m_equippedItem;
    private Collider m_riggerItem;
    private Coroutine m_interactionCoroutine;

    [Header("RayCast Distance")]
    [SerializeField] private float m_raycastDistance;
    
    //# 문 등 상호작용 동작
    // 1. 인터렉션이 가능한 영역으로 들어감
    // 2. 예를 들어 문이 있는 곳에서 e(인터렉션키)를 누름
    // 3. Trigger 내에서 동자r

    void Update()
    {
        DetectItemByRay();

        if (m_riggerItem != null && GameManager.Instance.Input.InteractionKeyPressed)
        {
            if (StoreItem(m_riggerItem.gameObject))
                m_riggerItem = null;
        }
        if(GameManager.Instance.Input.InteractionKeyPressed)
            InteractWithInteractableObject();

        for (int i = 0; i < GameManager.Instance.Input.ItemKeyPressed.Length; i++)
        {
            if (GameManager.Instance.Input.ItemKeyPressed[i])
            {
                EquipItem(i);
                GameManager.Instance.Input.ItemKeyPressed[i] = false;
            }
        }

        if (GameManager.Instance.Input.DropKeyPressed)
        {
            DropItem();
        }
    }

    void DetectItemByRay()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 direction = transform.forward;

        if (Physics.SphereCast(origin, 0.4f, direction, out RaycastHit hit, 1.5f))
        {
            if (hit.collider.CompareTag("Items"))
            {
                m_riggerItem = hit.collider;
                return;
            }
        }

        m_riggerItem = null;
    }

    public bool StoreItem(GameObject item)
    {
        ItemInfo info = item.GetComponent<ItemInfo>() ?? item.GetComponentInChildren<ItemInfo>();
        if (info == null)
        {
            Debug.LogWarning("ItemInfo 컴포넌트를 찾을 수 없습니다: " + item.name);
            return false;
        }

        for (int i = 0; i < m_itemSlots.Length; i++)
        {
            if (m_itemSlots[i].ItemObject == null)
            {
                m_itemSlots[i] = new ItemInfo.ItemSlotData
                {
                    ItemName = info.ItemName,
                    Type = info.Type,
                    Values = info.GetAttributeDictionary(),
                    ItemObject = item
                };

                item.SetActive(false);
                item.name = item.name.Replace("(Clone)", "").Trim();
                Debug.Log($"{i + 1}번 슬롯에 {item.name} 저장 완료");
                return true;
            }
        }

        Debug.Log("슬롯이 모두 찼습니다!");
        return false;
    }

    public void EquipItem(int slotIndex)
    {
        if (m_itemSlots[slotIndex].ItemObject == null)
        {
            Debug.LogWarning($"[{slotIndex + 1}]번 슬롯에 장착 가능한 아이템이 없습니다.");
            return;
        }

        if (m_equippedSlotIndex == slotIndex)
        {
            UnequipItem();
            return;
        }

        UnequipItem();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다.");
            return;
        }

        m_equippedItem = Instantiate(m_itemSlots[slotIndex].ItemObject, player.transform);
        m_equippedItem.SetActive(true);
        m_equippedItem.transform.localPosition = Vector3.zero;
        m_equippedItem.transform.localRotation = Quaternion.identity;

        m_equippedSlotIndex = slotIndex;
        Debug.Log($"{slotIndex + 1}번 슬롯 장착 완료");
    }

    private void UnequipItem()
    {
        if (m_equippedItem != null)
        {
            Destroy(m_equippedItem);
            Debug.Log("아이템 장착 해제");
        }

        m_equippedItem = null;
        m_equippedSlotIndex = -1;
    }

    public void DropItem()
    {
        if (m_equippedItem == null || m_equippedSlotIndex == -1)
        {
            Debug.Log("버릴 아이템 없음");
            return;
        }

        GameObject dropped = Instantiate(m_itemSlots[m_equippedSlotIndex].ItemObject);
        dropped.SetActive(true);
        dropped.transform.position = transform.position + transform.right * 1f;

        Rigidbody rb = dropped.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        Debug.Log($"{dropped.name} 버림");
        m_itemSlots[m_equippedSlotIndex] = new ItemInfo.ItemSlotData();
        UnequipItem();
    }

    private void InteractWithInteractableObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * m_raycastDistance, Color.red, 0.1f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.distance > m_raycastDistance)
                return;
            
            Debug.Log(hitInfo.collider.gameObject.name);

            if (hitInfo.collider.gameObject.CompareTag("InteractableObject"))
            {
                IInteractable interactableObject = hitInfo.collider.gameObject.GetComponent<IInteractable>();
                interactableObject.Interact();
                m_interactionCoroutine = StartCoroutine(InteractionDelay());
            }
        }
        
    }
    
    IEnumerator InteractionDelay()
    {
        yield return new WaitForSeconds(0.1f);
        m_interactionCoroutine = null;
    }
}
