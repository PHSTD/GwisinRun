using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MonsterTest : MonoBehaviour
{
    public float detectRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;

    private Transform player;
    private NavMeshAgent agent;
    private IDamageable playerDamageable;
    private float lastAttackTime = 0f;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerDamageable = playerObj.GetComponent<PlayerHealth>();
        }
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null || agent == null || !agent.isOnNavMesh)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectRange)
        {
            agent.SetDestination(player.position);

            if (distance <= attackRange)
            {
                Attack();
            }
        }
        else
        {
            agent.ResetPath();
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            Debug.Log("몬스터가 플레이어를 공격했습니다!");

            if (playerDamageable != null)
            {
                playerDamageable.TakeDamage(attackDamage);
                // TODO: 게임 오버씬 전환 필요
                Application.Quit(); // 게임 종료
            }
        }
    }
}
