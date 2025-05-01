using System;
using System.Collections;
using System.Dynamic;
using JetBrains.Annotations;
using UnityEngine;

public enum MonsterState
{
    Patrol,
    Chase,
    Search,
    Attack
}

public class MonsterPatrolState : FsmState<MonsterState>
{
    private Monster monster;

    private int currentEnemyPosition;
    float walkingPointRadius = 3; // 순찰 지점 도달 판정에 사용되는 반경
    public MonsterPatrolState(Monster monster) : base(MonsterState.Patrol)
    {
        this.monster = monster;
    }

    public override void OnEnter(MonsterState from, FsmMessage msg)
    {
        Debug.Log("MonsterPatrolState 시작.");
        if(monster.walkPoints.Length <= 1)
        {
            //포인트가 없으므로 추가해야합니다.
            Debug.Log("포인트가 없으므로 추가해야합니다.");
        }
        monster.StartCustomCoroutine(PatrolRoutine());
    }

    public override void OnExit(MonsterState to)
    {
        // 상태 종료 시 코루틴 정지
        monster.StopCustomCoroutine();
    }

    private void repeatPatrol()
    {
        if (Vector3.Distance(monster.walkPoints[currentEnemyPosition].transform.position, 
            monster.transform.position) < walkingPointRadius)
        {
            // 다음 순찰 지점을 무작위로 선택
            currentEnemyPosition = UnityEngine.Random.Range(0, monster.walkPoints.Length);
            // 배열의 범위를 넘어선 경우 인덱스를 초기화
            if (currentEnemyPosition >= monster.walkPoints.Length)
            {
                currentEnemyPosition = 0;
            }
        }
        // 선택된 지점으로 이동 명령을 내림
        monster.navMesh.SetDestination(monster.walkPoints[currentEnemyPosition].transform.position);
    }

    // 상태별 코루틴 로직
    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            Transform target = monster.fieldOfViewSystem.FindVisibleTarget();
            if(target != null)
            {
                monster.FSM.ChangeState(MonsterState.Chase);
                yield break;
            }
            else
            {
                if(monster.IsArrived())
                {
                    repeatPatrol();
                }
            }
            yield return new WaitForSeconds(monster.delay);
        }
    }
}

public class MonsterSearchState : FsmState<MonsterState>
{
    private Monster monster;
    private int m_searchCount;
    
    public MonsterSearchState(Monster monster) : base(MonsterState.Search)
    {
        this.monster = monster;
    }
    public override void OnEnter(MonsterState fromState, FsmMessage msg)
    {
        monster.StartCustomCoroutine(SearchRoutine());
        m_searchCount = 0;
    }


    public override void OnExit(MonsterState toState)
    {
        monster.StopCustomCoroutine();
    }
    private IEnumerator SearchRoutine()
    {
        while (true)
        {
            Transform target = monster.fieldOfViewSystem.FindVisibleTarget();
            if(target != null)
            {
                monster.FSM.ChangeState(MonsterState.Chase);
                yield break; // 코루틴 명시적 종료
            }
            else if(monster.IsArrived())
            {
                monster.MoveToRandomSearchPoint();
                ++m_searchCount;
                Debug.Log($"Search Count: {m_searchCount}/{monster.searchMoveCount}");
                if(m_searchCount > monster.searchMoveCount)
                {
                    monster.FSM.ChangeState(MonsterState.Patrol);
                    yield break; // 코루틴 명시적 종료
                }
            }
            yield return new WaitForSeconds(monster.delay);
        }
    }

}


public class MonsterChaseState : FsmState<MonsterState>
{
    private Monster monster;
    public MonsterChaseState(Monster monster) : base(MonsterState.Chase)
    {
        this.monster = monster;
    }
    public override void OnEnter(MonsterState fromState, FsmMessage msg)
    {
        monster.StartCustomCoroutine(ChaseRoutine());
    }


    public override void OnExit(MonsterState toState)
    {
        monster.StopCustomCoroutine();
    }
    private IEnumerator ChaseRoutine()
    {
        while (true)
        {
            Transform target = monster.fieldOfViewSystem.FindVisibleTarget();
            if(target != null)
            {
                monster.navMesh.SetDestination(target.position);
            }
            else if(monster.IsArrived())
            {
                
                monster.FSM.ChangeState(MonsterState.Search);
                yield break;
            }
            yield return new WaitForSeconds(monster.delay);
        }
    }
}


public class MonsterAttackState : FsmState<MonsterState>
{
    private Monster monster;
    public MonsterAttackState(Monster monster) : base(MonsterState.Attack)
    {
        this.monster = monster;
    }
    public override void OnEnter(MonsterState fromState, FsmMessage msg)
    {
        monster.StartCustomCoroutine(Attack());
    }


    public override void OnExit(MonsterState toState)
    {
        monster.StopCustomCoroutine();
    }

    private IEnumerator Attack()
    {
        Debug.Log("공격 시작");
        yield return new WaitForSeconds(3f);
        Debug.Log("공격 종료");
        yield break;
    }
}