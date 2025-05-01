using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class Monster : MonoBehaviour
{
    [Header ("MonsterPatrolState")]
    public GameObject[] walkPoints; // 순찰할 위치들을 담은 배열

    [Header ("MonsterSearchState")]
    public float searchRadius = 5f;         //탐색 반경
    public int searchMoveCount = 3;         //탐색 횟수
    

    public StateMachine<MonsterState> FSM { get; private set; }
    private Coroutine customCoroutine;
    public float    delay;
    public FieldOfViewSystem fieldOfViewSystem;
    public NavMeshAgent navMesh;
    private void Awake()
    {
        FSM = new StateMachine<MonsterState>();
        FSM.AddState(new MonsterPatrolState(this));
        FSM.AddState(new MonsterChaseState(this));
        FSM.AddState(new MonsterSearchState(this));
        FSM.AddState(new MonsterAttackState(this));
        FSM.ChangeState(MonsterState.Patrol);
    }
    // 0.2초마다 실행되는 추적 및 상태별 추가 동작 코루틴

    // 상태에서 코루틴 시작 요청
    public void StartCustomCoroutine(IEnumerator routine)
    {
        StopCustomCoroutine();
        customCoroutine = StartCoroutine(routine);
    }

    // 상태에서 코루틴 정지 요청
    public void StopCustomCoroutine()
    {
        if (customCoroutine != null)
        {
            StopCoroutine(customCoroutine);
            customCoroutine = null;
        }
    }

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
}