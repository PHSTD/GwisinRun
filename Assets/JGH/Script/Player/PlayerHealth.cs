using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("HP")]
    public static int MaxHealth = 100; // 최대 체력
    public static int CurrentHealth; // 현재 체력
    
    [Header("Stamina")]
    public static int MaxStamina = 100; // 최대 스태미너
    public static int CurrentStamina = 100; // 현재 스태미너
    
    static float m_timer = 0f;
    
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        Debug.Log($"플레이어가 {amount} 데미지를 입었습니다. 현재 체력: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        Debug.ClearDeveloperConsole(); 
        Debug.Log($"MaxH{MaxHealth}");
        Debug.Log($"CurrH{CurrentHealth}");
        Debug.Log($"MaxS{MaxStamina}");
        Debug.Log($"CurrS{CurrentStamina}");
    }

    // TODO: 게임 오버 씬으로 전환 필요
    public static void Die()
    {
        Debug.Log("플레이어 사망!");
    }

    
    public static void StaminaPlus()
    {
        // 일정 시간 간격으로 스태미너 증가 처리 (0.05초마다 1씩 증가)
        m_timer += Time.deltaTime;
        if (m_timer >= 0.05f)
        {
            CurrentStamina++;
            m_timer = 0f;
        }
        if (CurrentStamina >= 100) CurrentStamina = 100;
    }

    public static void StaminaMinus()
    {
        // 일정 시간 간격으로 스태미너 감소 처리 (0.05초마다 1씩 감소)
        m_timer += Time.deltaTime;
        if (m_timer >= 0.05f)
        {
            CurrentStamina--;
            m_timer = 0f;
        }

        // 스태미너가 0 이하일 경우 걷는 속도로 제한
        if (CurrentStamina <= 0)
        {
            CurrentStamina = 0;

            // 완전히 탈진하면 강제로 걷기 속도로 고정
            PlayerMove.m_speed = PlayerMove.m_walkSpeed;
            return;
        }

        // 현재 스태미너 비율 
        float staminaPercent = (float)CurrentStamina / MaxStamina;

        // 스태미너가 20% 이하일 경우에만 속도 보간 적용
        if (staminaPercent <= 0.3f)
        {
            // 0% ~ 20% 구간
            float t = staminaPercent / 0.3f;

            // 걷기 속도와 달리기 속도 사이를 보간
            // t가 1이면 runSpeed, t가 0이면 walkSpeed
            float targetSpeed = Mathf.Lerp(PlayerMove.m_walkSpeed, 10f, t); // 10f는 달리기 속도

            // 보간된 속도를 적용
            PlayerMove.m_speed = targetSpeed;
        }
    }
}
