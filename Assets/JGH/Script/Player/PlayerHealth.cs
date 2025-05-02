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
        GameManager.Instance.Inventory.OnUseItem.AddListener(UseItem);
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

    void Die()
    {
        Debug.Log("플레이어 사망!");
    }

    private void UseItem(string itemName, int value)
    {
        if (itemName != "HeartPotion")
            return;
        
        CurrentHealth += value;

        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
            //todo GameOver;
        }
        else if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
    
    public static void StaminaPlus()
    {
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
        m_timer += Time.deltaTime;
        if (m_timer >= 0.05f)
        {
            CurrentStamina--;
            m_timer = 0f;
        }
        if (CurrentStamina <= 0)
        {
            CurrentStamina = 0;
            PlayerMove.m_speed = PlayerMove.m_walkSpeed;
        }
    }
}
