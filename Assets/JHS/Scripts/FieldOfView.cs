using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("View Settings")]
    [SerializeField] private float m_ViewRadius = 10f;
    [SerializeField] [Range(0, 360)] private float m_ViewAngle = 90f;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask m_TargetMask;
    [SerializeField] private LayerMask m_ObstacleMask;

    [Header("Detection")]
    public List<Transform> visibleTargets = new List<Transform>();

    // 마지막 탐지 정보를 저장하는 속성들
    public Vector3 lastSeenDirection { get; private set; } = Vector3.zero;
    public Vector3 lastSeenPosition { get; private set; } = Vector3.zero;
    public Vector3 lastObstaclePosition { get; private set; } = Vector3.zero;

    // 탐지 주기
    [SerializeField] private float m_DetectionDelay = 0.2f;

    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(m_DetectionDelay));
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        WaitForSeconds wait = new WaitForSeconds(delay);
        
        while(true)
        {
            yield return wait;
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        visibleTargets.Clear();
        
        // 시야 반경 내의 타겟 검색
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, m_ViewRadius, m_TargetMask);

        for(int i = 0; i < targetsInViewRadius.Length; ++i)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            
            // 시야각 내에 있는지 확인
            if(Vector3.Angle(transform.forward, dirToTarget) < m_ViewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                
                // 장애물에 가려져 있는지 확인
                if(!Physics.Raycast(transform.position, dirToTarget, out RaycastHit hit, dstToTarget, m_ObstacleMask))
                {
                    // 타겟이 시야에 들어옴
                    visibleTargets.Add(target);
                    
                    // 플레이어를 발견했을 때 정보 저장
                    lastSeenDirection = dirToTarget;
                    lastSeenPosition = target.position;
                }
                else
                {
                    // 장애물에 가려진 경우
                    lastObstaclePosition = hit.point;
                }
            }
        }
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