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
    
    //# 수정 사항(20250503) -- 시작
    private void Update()
    {
        if (CanStandUp())
        {
            Clear();
        }
        else
        {
            DetectedObjectAtHead();
        }
    }
    private bool CanStandUp()
    {
        // 플레이어 머리 위 공간 체크
        float checkRadius = 0.3f;
        Vector3 checkPosition = transform.position + Vector3.up;
        return !Physics.CheckSphere(checkPosition, checkRadius);
    }
    //# 수정 사항(20250503) -- 끝
}
