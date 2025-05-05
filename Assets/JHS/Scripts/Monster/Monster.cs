using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class Monster : MonoBehaviour
{
    
    public Animator animator;
    
    [Header ("MonsterPatrolState")]
    public GameObject[] walkPoints;             // 순찰할 위치들을 담은 배열

    [Header ("MonsterSearchState")]
    public float    searchRadius        = 5f;       //탐색 반경
    public int      searchMoveCount     = 3;        //탐색 횟수

    [Header("Behavior Timing")]
    public float    stateTickDelay = 0.2f;

    [Header("References")]
    public NavMeshAgent         navMesh;
    [Header("Movement")]
    public float rotationSpeed = 5f; // 회전 속도
    //===========================================================
    [Header("Combat Settings")]
    public float attackRange = 2.0f;
    public float attackCooldown = 2.0f;
    private bool isAttacking = false; // 공격 중 상태
    private float lastAttackTime = 0f;

    private string m_currentState;

    //==========================================추가 5월5일

    private IMonsterState m_currentStateInstance;
    
    private MonsterChase m_monsterChase;
    private MonsterSearch m_monsterSearch;
    private MonsterPatrol m_monsterPatrol;
    private MonsterAttack m_monsterAttack;
    public IMonsterState GetChaseState() => m_monsterChase;
    public IMonsterState GetSearchState() => m_monsterSearch;
    public IMonsterState GetMonsterPatrol() => m_monsterPatrol;
    public IMonsterState GetMonsterAttack() => m_monsterAttack;
    
    // Monster.cs에 추가
    [Header("Door Detection")]
    public float doorWaitTime = 5f;  // 문 앞에서 대기할 시간
    private Transform lastDoorEntered;

    private Coroutine customCoroutine;
    
    public void SetCurrentStateString(string stateName) => m_currentState = stateName;
    
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private LayerMask targetLayer;
    
    private Transform currentTarget;
    public Transform CurrentTarget
    {
        get => currentTarget;
        set => currentTarget = value;
    }
    
    public int attackPower = 20; // 공격력
    
    // // 탐지 주기
    [SerializeField] private float m_DetectionDelay = 0.5f;
    [SerializeField] private float m_ViewRadius = 10f;
    [SerializeField] private float m_ViewAngle = 90f;
    [SerializeField] private LayerMask m_TargetMask;
    [SerializeField] private LayerMask m_ObstacleMask;
    
    
    // 속성 접근자 추가
    public float ViewRadius => m_ViewRadius;
    public float ViewAngle => m_ViewAngle;
    
    
    private void Awake()
    {
        m_monsterPatrol = new MonsterPatrol();
        m_monsterPatrol.SetMonster(this);

        m_monsterAttack = new MonsterAttack();
        m_monsterAttack.SetMonster(this);

        m_monsterChase = new MonsterChase();
        m_monsterChase.SetMonster(this);

        m_monsterSearch = new MonsterSearch();
        m_monsterSearch.SetMonster(this);
        
        navMesh = GetComponent<NavMeshAgent>();
        
        animator = GetComponent<Animator>();
        
    }
    
    public Transform FindVisibleTarget()
    {
        // 시야 반경 내의 타겟 검색
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, m_ViewRadius, m_TargetMask);

        foreach (var collider in targetsInViewRadius)
        {
            Transform target = collider.transform;

            if (IsTargetVisible(target))
            {
                return target;
            }
        }
        return null;
    }
    
    private bool IsTargetVisible(Transform target)
    {
        Vector3 origin = transform.position;
        Vector3 dirToTarget = (target.position - origin);
        float sqrDistanceToTarget = dirToTarget.sqrMagnitude;

        // 시야각 체크
        float angleToTarget = Vector3.Angle(transform.forward, dirToTarget.normalized);
        if (angleToTarget > m_ViewAngle * 0.5f)
            return false;

        // 시야 거리 체크
        if (sqrDistanceToTarget > m_ViewRadius * m_ViewRadius)
            return false;

        // 장애물 체크 (Raycast)
        if (Physics.Raycast(origin, dirToTarget.normalized, out RaycastHit hit, Mathf.Sqrt(sqrDistanceToTarget), m_ObstacleMask))
        {
            // 장애물이 타겟보다 앞에 있는 경우
            if (hit.transform != target)
                return false;
        }

        return true;
    }

    private void Start()
    {
        ChangeState(m_monsterPatrol);
        m_currentState = "Patrol";
    }

    private void Update()
    {
        
        LookAtTarget(currentTarget);
        HandleAttackDistance(); // 또는 거리 체크 등
        
        if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
        {
            navMesh.isStopped = true;
        }
        else
        {
            navMesh.isStopped = false;
        }
    }
    
    private void HandleAttackDistance()
    {
        if (currentTarget == null) return;

        float distance = Vector3.Distance(transform.position, currentTarget.position);

        if (!isAttacking && CanAttack() && distance <= attackRange && m_currentState == "Chase")
        {
            Debug.Log("▶ 공격 상태 전환 시도");
            ChangeState(m_monsterAttack);
            m_currentState = "Attack";
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

    public void SetAttacking(bool attacking)
    {
        isAttacking = attacking;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    // 공격 시작 시 호출
    public void StartAttack()
    {
        lastAttackTime = Time.time;
        Debug.Log($"Monster attack started, next attack available at: {lastAttackTime + attackCooldown}");
    }
    
    public void ChangeState(IMonsterState newState)
    {
        if (isAttacking && newState != m_monsterAttack)
            return;

        if (m_currentStateInstance != null)
            m_currentStateInstance.OnExit();

        m_currentStateInstance = newState;
        m_currentStateInstance.OnEnter();
    }

    #region Coroutine Control
    public void StartCustomCoroutine(IEnumerator routine)
    {
        StopCustomCoroutine();
        customCoroutine = StartCoroutine(routine);
    }

    public void StopCustomCoroutine()
    {
        if (customCoroutine != null)
        {
            StopCoroutine(customCoroutine);
            customCoroutine = null;
        }
    }
    #endregion
    
    // 상태에서 코루틴 정지 요청
    public bool IsArrived()
    {
        return !navMesh.pathPending &&
            navMesh.remainingDistance <= navMesh.stoppingDistance &&
            (!navMesh.hasPath || navMesh.velocity.sqrMagnitude == 0f);
    }


    public void MoveToRandomSearchPoint()
    {
        Vector3 forwardDir = transform.forward;
        Vector3 randomOffset = Random.insideUnitSphere * searchRadius;
        randomOffset.y = 0;

        Vector3 biasedDirection = forwardDir * 2f + randomOffset;
        Vector3 randomPoint = transform.position + biasedDirection; //lastKnownPosition

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, searchRadius, NavMesh.AllAreas))
        {
            navMesh.SetDestination(hit.position);
        }
    }

    public void LookAtTarget(Transform target)
    {
        if (target == null) return;
        
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // y축 회전만 적용
        
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                lookRotation, 
                Time.deltaTime * rotationSpeed); // 회전 속도 조절
        }
    }
    
    
}