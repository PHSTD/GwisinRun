using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Game UI")]
    [SerializeField] private GameObject m_pausedMenu;
    public static GameObject PausedMenu;
    
    //# 수정 사항(20250502) -- 시작
    [SerializeField] private GameObject m_gameStartPanel;
    public static GameObject GameStartPanel;
    //# SDW 수정 사항(20250502) -- 끝
    
    [Header("Basic Setting")]
    public static CharacterController PlayerCont;
    public static Transform PlayerTransform;
    public static PlayerHide HeadTriggerObject;

    private void Awake()
    {
        PausedMenu = m_pausedMenu;
    }

    void Start()
    {
        GameManager.Instance.Inventory.OnUseItem.AddListener(UseItem);
        
    }

    private void UseItem(string itemName, int value)
    {
        if (itemName != "SpeedPotion")
            return;
        
        PlayerHealth.CurrentStamina += value;

        if (PlayerHealth.CurrentStamina < 0)
        {
            PlayerHealth.CurrentStamina = 0;
        }
        else if (PlayerHealth.CurrentStamina > PlayerHealth.MaxStamina)
        {
            PlayerHealth.CurrentStamina = PlayerHealth.MaxStamina;
        }
    }
}
