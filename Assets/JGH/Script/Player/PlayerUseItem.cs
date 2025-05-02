using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.Inventory.OnUseItem.AddListener(UseItem);
    }

    void Update()
    {
        for (int i = 0; i < GameManager.Instance.Input.ItemKeyPressed.Length; i++)
        {
            if (GameManager.Instance.Input.ItemKeyPressed[i])
            {
                GameManager.Instance.Inventory.UseItem(i);
                //# 현재 방법을 찾지 못해 읽은 후 수동으로 clear 해야 합니다.
                //# 추후 리팩토링 예정
                GameManager.Instance.Input.ItemKeyPressed[i] = false;
            }
        }
    }
    
    public void UseItem(string itemName, int value)
    {
        switch (itemName)
        {
            case "SpeedPotion":
                PlayerHealth.CurrentStamina += value;

                if (PlayerHealth.CurrentStamina < 0)
                {
                    PlayerHealth.CurrentStamina = 0;
                }
                else if (PlayerHealth.CurrentStamina > PlayerHealth.MaxStamina)
                {
                    PlayerHealth.CurrentStamina = PlayerHealth.MaxStamina;
                }
                break;
            
            case "HeartPotion":
                PlayerHealth.CurrentHealth += value;

                if (PlayerHealth.CurrentHealth < 0)
                {
                    PlayerHealth.CurrentHealth = 0;
                    PlayerHealth.Die();
                }
                else if (PlayerHealth.CurrentHealth > PlayerHealth.MaxHealth)
                {
                    PlayerHealth.CurrentHealth = PlayerHealth.MaxHealth;
                }
                break;
                
            default: 
                
                break;
        }
    }
}
