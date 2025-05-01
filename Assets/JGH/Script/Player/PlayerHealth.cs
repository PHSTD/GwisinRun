using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int m_maxHealth = 100;
    [SerializeField] private int m_currentHealth;

    void Start()
    {
        m_currentHealth = m_maxHealth;
        GameManager.Instance.Inventory.OnUseItem.AddListener(UseItem);
    }

    public void TakeDamage(int amount)
    {
        m_currentHealth -= amount;
        Debug.Log($"플레이어가 {amount} 데미지를 입었습니다. 현재 체력: {m_currentHealth}");

        if (m_currentHealth <= 0)
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
        
        m_currentHealth += value;

        if (m_currentHealth < 0)
        {
            m_currentHealth = 0;
            //todo GameOver;
        }
        else if (m_currentHealth > m_maxHealth)
        {
            m_currentHealth = m_maxHealth;
        }
    }
}
