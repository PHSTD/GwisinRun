using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIneration : MonoBehaviour
{
    [Header("RayCast Distance")]
    [SerializeField] private float m_raycastDistance;
    
    private Coroutine m_interactionCoroutine;
    private GameObject m_detectedObject;

    
    void Update()
    {
        //# 수정 사항(20250502) -- 시작
        // if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
        //     return;
        //# 수정 사항(20250502) -- 끝
        
        if (GameManager.Instance.Input.InteractionKeyPressed)
        {
            DetectInteractableObjectByRay();
            InteractWithInteractableObject();
        }

        if (GameManager.Instance.Input.DropKeyPressed)
        {
            DropItem();
        }
    }

    void DetectInteractableObjectByRay()
    {
        //todo Player가 Raycast 쏨 (InteractWithInteractableObject)
        //todo 문 등인지, 아이템인지 구분하여 처리가 필요함
        //todo 아이템일 경우
        //     ㄴ 마우스(화면 중앙)가 아이템으로 향하면 지정된 거리 이내면 아이템 정보가 팝업
        //     ㄴ 지정된 거리 이내에서 E(상호작용키)를 누르면 아이템을 먹어야 함
        //     ㄴ 인벤토리에 아이템 추가
        //     ㄴ 인벤토리에서 아이템 사용
        //     ㄴ 아이템 버리기
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * m_raycastDistance, Color.red, 0.1f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.distance > m_raycastDistance)
            {
                m_detectedObject = null;
                return;
            }
            
            m_detectedObject = hitInfo.collider.gameObject;
        }
    }

    // public bool StoreItem(GameObject item)
    // {
    // }

    public void DropItem()
    {
        // if (m_equippedItem == null || m_equippedSlotIndex == -1)
        // {
        //     Debug.Log("버릴 아이템 없음");
        //     return;
        // }
        //
        // GameObject dropped = Instantiate(m_itemSlots[m_equippedSlotIndex].ItemObject);
        // dropped.SetActive(true);
        // dropped.transform.position = transform.position + transform.right * 1f;
        //
        // Rigidbody rb = dropped.GetComponent<Rigidbody>();
        // if (rb != null)
        // {
        //     rb.isKinematic = false;
        //     rb.useGravity = true;
        // }
        //
        // Debug.Log($"{dropped.name} 버림");
        // m_itemSlots[m_equippedSlotIndex] = new ItemInfo.ItemSlotData();
    }

    private void InteractWithInteractableObject()
    {
        if (m_detectedObject == null)
            return;
        
        if (m_detectedObject.CompareTag("InteractableObject") || m_detectedObject.CompareTag("Item"))
        {
            IInteractable interactableObject = m_detectedObject.gameObject.GetComponent<IInteractable>();
            if (m_interactionCoroutine == null)
            {
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
