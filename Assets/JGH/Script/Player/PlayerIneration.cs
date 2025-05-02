using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIneration : MonoBehaviour
{
    [Header("RayCast Distance")]
    private float m_raycastDistance = 1.8f;
    
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(ray.origin, ray.direction * m_raycastDistance, Color.red, 0.1f);
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

    // TODO: 아이템 버리기 구현 필요
    public void DropItem()
    {
        
    }
    
    IEnumerator InteractionDelay()
    {
        yield return new WaitForSeconds(0.1f);
        m_interactionCoroutine = null;
    }

}
