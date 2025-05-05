using System.Collections;
using UnityEngine;

public class MonsterChase : IMonsterState 
{
    private Monster monster;
    private float chaseTimer;
    private float maxChaseTime = 5f;
    

    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }

    private IEnumerator ChaseRoutine()
    {
        while (true)
        {
            if (monster.CurrentTarget != null)
            {
                Transform target = monster.CurrentTarget;
                monster.navMesh.SetDestination(target.position);
                
                // OnTargetDistanceChanged 이벤트가 제대로 동작하지 않을 경우를 대비한 보조 로직
                float distanceToTarget = Vector3.Distance(monster.transform.position, target.position);
                if (distanceToTarget <= monster.attackRange && monster.CanAttack() && !monster.IsAttacking())
                {
                    Debug.Log("ChaseRoutine detected attack range - changing to Attack state");
                    monster.ChangeState(monster.GetMonsterAttack());
                    monster.SetCurrentStateString("Attack");
                    yield break;
                }
            }
            
            // 추격 제한 시간 초과
            chaseTimer += monster.stateTickDelay;
            if (chaseTimer >= maxChaseTime)
            {
                monster.CurrentTarget = null;
                monster.ChangeState(monster.GetSearchState());
                monster.SetCurrentStateString("Search");
                yield break;
            }
            
            yield return new WaitForSeconds(monster.stateTickDelay);
        }
    }

    public void OnEnter()
    {
        monster.StartCustomCoroutine(ChaseRoutine());
    }

    public void OnExit()
    {
        monster.StopCustomCoroutine();
    }

    public void OnUpdate()
    {
    }
    
}
