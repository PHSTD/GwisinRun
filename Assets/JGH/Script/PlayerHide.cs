using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    [SerializeField] private bool m_isHiding = false;
    [SerializeField] private bool m_canHide = false;
    [SerializeField] private Camera m_playerCamera;
    [SerializeField] private Color m_originalColor;

    void Start()
    {
        m_playerCamera = Camera.main;
        m_originalColor = m_playerCamera.backgroundColor;
    }

    void Update()
    {
        if (m_canHide && Input.GetKeyDown(KeyCode.E))
        {
            if (!m_isHiding) Hide();
            else Unhide();
        }
    }

    void Hide()
    {
        m_isHiding = true;
        m_playerCamera.backgroundColor = Color.black; // 검은 화면
        // 플레이어 이동, 상호작용 등 비활성화 처리 추가(필요시)
    }

    void Unhide()
    {
        m_isHiding = false;
        m_playerCamera.backgroundColor = m_originalColor;
        // 플레이어 이동, 상호작용 등 다시 활성화(필요시)
    }

    // 트리거로 HideObject 감지
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HideObject"))
        {
            m_canHide = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HideObject"))
        {
            m_canHide = false;
            if (m_isHiding) Unhide(); // 숨은 상태에서 벗어나면 자동 해제
        }
    } 
}
