using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : Monster, IMonsterState 
{
    
    private Monster monster;

    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }
    
    public IEnumerator Attack()
    {
        Debug.Log("▶ 공격 시작");
        monster.animator.SetTrigger("IsAttacking");

        // 애니메이션 타이밍에 맞춘 대기
        yield return new WaitForSeconds(1.5f);

        if (monster.CurrentTarget != null)
        {
            float distance = Vector3.Distance(monster.transform.position, monster.CurrentTarget.position);
            Debug.Log($"▶ 공격 거리 체크: {distance} / {monster.attackRange * 1.2f}");

            if (distance <= monster.attackRange * 1.2f)
            {
                IDamageable damageable = monster.CurrentTarget.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(monster.attackPower);
                    Debug.Log("✅ 데미지 적용!");
                }
                else
                {
                    Debug.LogWarning("⚠️ 대상이 IDamageable이 아님");
                }
            }
        }
        else
        {
            Debug.LogWarning("⚠️ CurrentTarget이 null임");
        }

        yield return new WaitForSeconds(1.5f);

        Debug.Log("▶ 공격 종료");

        // 다음 상태로 전환
        if (monster.CurrentTarget != null)
        {
            monster.ChangeState(monster.GetChaseState());
            monster.SetCurrentStateString("Chase");
        }
        else
        {
            monster.ChangeState(monster.GetSearchState());
            monster.SetCurrentStateString("Search");
        }
    }

    public void OnEnter()
    {
        Debug.Log("MonsterAttackState 시작");
        monster.SetAttacking(true);
        monster.StartAttack(); // 공격 시작 시간 기록
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
        throw new System.NotImplementedException();
    }
}
