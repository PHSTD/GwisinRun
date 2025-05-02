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
    // PlayerController
    public static CharacterController PlayerCont;
    // Player 위치
    public static Transform PlayerTransform;
    // 머리 충돌 판정 오브젝트
    public static PlayerHide HeadTriggerObject;

    private void Start()
    {
        PlayerCont = GetComponent<CharacterController>();
        
    }

    //# 수정 사항(20250502) -- 시작
    void Update()
    {
        CheckPauseKeyPressed();
    }

    private void CheckPauseKeyPressed()
    {
        if (GameManager.Instance.Input.PauseKeyPressed)
        {
            if (PlayerController.GameStartPanel != null && PlayerController.GameStartPanel.activeSelf == true)
                return;
            
            if (GameManager.Instance.IsPaused == false)
            {
                PlayerController.PausedMenu.SetActive(true);
            }
            else
            {
                PlayerController.PausedMenu.SetActive(false);
                PlayerController.PausedMenu.GetComponent<PauseMenuUI>().Close();
            }
        }
    } 
    //# 수정 사항(20250502) -- 끝

    private void UseItem(string itemName, int value)
    {
        //# 수정 사항(20250502) -- 시작
        if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
            return;
        //# 수정 사항(20250502) -- 끝
        
        if (itemName != "SpeedPotion")
            return;
        
    }
}
