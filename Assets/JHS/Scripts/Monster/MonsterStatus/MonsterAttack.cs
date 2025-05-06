using System.Collections;
using UnityEngine;

public class MonsterAttack : IMonsterState 
{
    
    private Monster monster;

    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }
    
    public IEnumerator Attack()
    {
        while (monster.GetCurrentStateInstance() == monster.GetAttackState())
        {
            monster.animator.SetTrigger("IsAttacking");
            if (monster.CurrentTarget != null)
            {
                // 반복 간격
                yield return new WaitForSeconds(1f);

                float distance = Vector3.Distance(monster.transform.position, monster.CurrentTarget.position);
                
                if (distance > monster.attackRange * 1.5f)
                {
                    Debug.Log("Search 상태로 이동 조건 충족");
                    // 너무 멀어졌을 경우 Search 상태로 전환
                    monster.ChangeState(monster.GetSearchState());
                    Debug.Log("yield break 직전 상태 확인: " + monster.GetCurrentStateInstance()?.GetType().Name);
                    yield break;
                }

                if (distance <= monster.attackRange * 1.2f && monster.CanAttack())
                {
                    // 너무 멀어졌을 경우 Search 상태로 전환 
                    IDamageable damageable = monster.CurrentTarget.GetComponent<IDamageable>();
                    if (damageable != null)
                    {

                        damageable.TakeDamage(monster.attackPower, monster.transform);
                    }

                    monster.StartAttack(); // 쿨다운 시간 초기화
                }
            }
            else
            {
                monster.ChangeState(monster.GetSearchState());
                yield break;
            }
        }
    }

    public void OnEnter()
    {
        Debug.Log(">> Attact 상태 진입");
        monster.SetAttacking(true);
        monster.StartAttack(); // 공격 시작 시간 기록
        
        // 애니메이션 속도 빠르게
        monster.animator.speed = 2.0f;
        
        monster.StartCustomCoroutine(Attack());
    }

    public void OnExit()
    {
        Debug.Log("MonsterAttackState 종료");
        monster.SetAttacking(false); // 중요: 공격 상태 해제
        
        monster.StopCustomCoroutine();
    }

    public void OnUpdate()
    {
    }
}
