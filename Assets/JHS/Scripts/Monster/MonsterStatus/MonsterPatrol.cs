using System.Collections;
using UnityEngine;

public class MonsterPatrol : Monster, IMonsterState 
{
    private Monster monster;

    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }


    private int currentEnemyPosition;
    float walkingPointRadius = 3; // 순찰 지점 도달 판정에 사용되는 반경


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
    public IEnumerator PatrolRoutine()
    {
        while (true)
        {
            Transform target = monster.FindVisibleTarget();
            if(target != null)
            {
                monster.ChangeState(monster.GetChaseState());
            }
            
            // 패트롤 로직
            if (monster.IsArrived())
            {
                repeatPatrol();
            }

            yield return new WaitForSeconds(monster.stateTickDelay);
        }
    }

    public void OnEnter()
    {
        
        Debug.Log("MonsterPatrolState 시작.");
        if(monster.walkPoints.Length <= 1)
        {
            //포인트가 없으므로 추가해야합니다.
            Debug.Log("포인트가 없으므로 추가해야합니다.");
        }
        monster.StartCustomCoroutine(PatrolRoutine());
    }

    public void OnExit()
    {
        monster.StopCustomCoroutine();
    }

    public void OnUpdate()
    {
    }
}
