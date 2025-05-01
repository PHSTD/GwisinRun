using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewSystem : MonoBehaviour
{
    [Header("View Settings")]
    [SerializeField] private float m_ViewRadius = 10f;
    [SerializeField] [Range(0, 360)] private float m_ViewAngle = 90f;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask m_TargetMask;
    [SerializeField] private LayerMask m_ObstacleMask;

    //public List<Transform> visibleTargets = new List<Transform>();

    // 탐지 주기
    [SerializeField] private float m_DetectionDelay = 0.2f;

    public Transform visibleTarget;
    private bool IsTargetVisible(Transform target)
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToTarget);

        if (angle < m_ViewAngle / 2f)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (!Physics.Raycast(transform.position, dirToTarget, distance, m_ObstacleMask))
            {
                return true;
            }
        }
        return false;
    }
    private void FindVisibleTargets(ref List<Transform> visibleTargets)
    {
        visibleTargets.Clear();
        
        //반경 내의 타겟들들
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, m_ViewRadius, m_TargetMask);

        foreach (var collider in targetsInViewRadius)
        {
            Transform target = collider.transform;
            // 시야각 내에 있는지 확인
            if (IsTargetVisible(target))
            {
                visibleTargets.Add(target);
                // 플레이어를 발견했을 때 정보 저장
                // lastSeenDirection = dirToTarget;
                // lastSeenPosition = target.position;
            }
            else
            {
                // 장애물에 가려진 경우
                //lastObstaclePosition = hit.point;
            }
        }
    }
    public Transform FindVisibleTarget()
    {
        visibleTarget = null;   // 수정해야함
        // 시야 반경 내의 타겟 검색
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, m_ViewRadius, m_TargetMask);

        foreach (var collider in targetsInViewRadius)
        {
            Transform target = collider.transform;

            if (IsTargetVisible(target))
            {   
                visibleTarget = target; // 수정해야함
                return target;
            }
        }
        return null;
    }

    /// <summary>
    /// 각도를 방향 벡터로 변환합니다.
    /// </summary>
    /// <param name="angleInDegrees">각도(도)</param>
    /// <param name="angleIsGlobal">전역 각도 여부</param>
    /// <returns>방향 벡터</returns>
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    // 속성 접근자 추가
    public float ViewRadius => m_ViewRadius;
    public float ViewAngle => m_ViewAngle;
}
