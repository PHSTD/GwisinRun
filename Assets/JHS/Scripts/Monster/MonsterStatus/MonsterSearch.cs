using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearch : IMonsterState 
{
    private Monster monster;

    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }
    
    private int m_searchCount;

    private IEnumerator SearchRoutine()
    {
        while (true)
        {
            Debug.Log(m_searchCount);
            Transform target = monster.FindVisibleTarget();
            if(target != null)
            {
                Debug.Log("MonsterSearchState Chase 상태로 변경");
                monster.ChangeState(monster.GetChaseState());
            }
            else if(monster.IsArrived())
            {
                monster.MoveToRandomSearchPoint();
                ++m_searchCount;
                Debug.Log($"Search Count: {m_searchCount}/{monster.searchMoveCount}");
                if(m_searchCount > monster.searchMoveCount)
                {
                    Debug.Log("MonsterSearchState Patrol 상태로 변경");
                    monster.ChangeState(monster.GetPatrolState());
                    yield break; // 코루틴 명시적 종료
                }
            }
            yield return new WaitForSeconds(monster.stateTickDelay);

        }
    }

    public void OnEnter()
    {
        m_searchCount = 0;
        monster.StartCustomCoroutine(SearchRoutine());
        Debug.Log("MonsterSearchState 실행됨 초기화됨됨" + m_searchCount);
    }

    public void OnExit()
    {
        Debug.Log("MonsterSearchState 종료됨");
        monster.StopCustomCoroutine();
    }

    public void OnUpdate()
    {
    }
}
