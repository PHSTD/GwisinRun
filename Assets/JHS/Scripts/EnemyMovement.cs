using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class EnemyMovement  : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private NavMeshAgent   m_agent;

    [Header("Detection")]
    [SerializeField] private FieldOfView    m_fieldOfView;
    [SerializeField] private Transform      m_playerTarget;
    [SerializeField] private float          m_lostSightAdvanceDistance = 3f;  // 플레이어가 숨은 방향으로 추가로 이동할 거리

    [Header("Search Settings")]
    [SerializeField] private float m_searchDuration = 5f;       //탐색 지속 시간
    [SerializeField] private float m_searchMoveInterval = 3f;   //랜덤 이동 간격
    [SerializeField] private float m_searchRadius = 5f;         //주변 몇 미터 안에서 랜덤 이동
    

    private Vector3     m_lastKnownPosition;
    private bool        m_isSearching = false;
    private float       m_searchTimer = 0f;
    private float       m_searchMoveTimer = 0f;
    private bool        m_isMoving = false;


    void Awake()
    {
        if (m_agent == null)
        {
            m_agent = GetComponent<NavMeshAgent>();
        }
    }

    void Start()
    {   
        if(m_fieldOfView == null)
            Debug.LogError("FieldOfView가 할당되지 않았습니다!");
        if(m_playerTarget == null)
            Debug.LogError("PlayerTarget 할당되지 않았습니다!");
    }

    // Update is called once per frame
    void Update()
    {   
        if(m_fieldOfView.visibleTargets.Count > 0)
        {
            // 시야에 플레이어가 있으면 추적
            ChasePlayer();
        }
        else if(!m_isMoving)
        {
            // 시야에 플레이어가 없고 이동이 완료된 상태면 탐색
            Search();
        }
        else
        {
            // 이동 중인 경우 도착 여부 확인
            CheckArrival();
        }
    }

    private void ChasePlayer()
    {
        // 플레이어의 마지막 위치 저장
        m_lastKnownPosition = m_fieldOfView.visibleTargets.First().position;
        // 플레이어가 향하던 방향으로 약간 더 이동
        Vector3 advancePos = m_lastKnownPosition + m_fieldOfView.lastSeenDirection * m_lostSightAdvanceDistance;
        m_agent.SetDestination(advancePos);
        
        m_isSearching = false;
        m_isMoving = true;
    }
    private void Search()
    {
        if (!m_isSearching)
        {
            // 탐색 모드 시작
            m_agent.SetDestination(m_lastKnownPosition);
            m_isSearching = true;
            m_searchTimer = m_searchDuration;
            m_searchMoveTimer = 0f;
            m_isMoving = true;
        }
        else//탐색중
        {
            // 탐색 중
            m_searchTimer -= Time.deltaTime;
            m_searchMoveTimer -= Time.deltaTime;

            if (m_searchTimer <= 0f)
            {
                // 탐색 시간 종료
                m_isSearching = false;
                m_agent.SetDestination(transform.position);
            }
            else if (m_searchMoveTimer <= 0f)
            {
                // 랜덤 위치로 이동
                MoveToRandomPoint();
                m_searchMoveTimer = m_searchMoveInterval;
                m_isMoving = true;
            }
        }
    }
    private void MoveToRandomPoint()
    {
        // 마지막 위치 주변의 랜덤한 지점으로 이동
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * m_searchRadius;
        randomDirection.y = 0;
        Vector3 randomPoint = m_lastKnownPosition + randomDirection;
        
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, m_searchRadius, NavMesh.AllAreas))
        {
            m_agent.SetDestination(hit.position);
        }
    }

    private void CheckArrival()
    {
        // 목적지에 도착했는지 확인
        if (!m_agent.pathPending && m_agent.remainingDistance <= m_agent.stoppingDistance)
        {
            if (!m_agent.hasPath || m_agent.velocity.sqrMagnitude == 0f)
            {
                Debug.Log("목적지에 도착! 마지막으로 본 방향: " + m_fieldOfView.lastSeenDirection);
                m_isMoving = false;
            }
        }
    }
}
