using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHide : MonoBehaviour
{
    //# 추후 리팩토링 시 SerializeField 제거
    [SerializeField] private bool m_isDetected = false;
    public bool IsDetected => m_isDetected;

    void OnEnable()
    {
        m_isDetected = false;
    }
    
    void DetectedObjectAtHead()
    {
        m_isDetected = true;
    }

    void Clear()
    {
        m_isDetected = false;
    }

    // 트리거로 HideObject 감지
    private void OnTriggerStay(Collider other)
    {
        DetectedObjectAtHead();
    }

    private void OnTriggerExit(Collider other)
    {
        Clear();
    } 
}
