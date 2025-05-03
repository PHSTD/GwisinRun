using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Game UI")]
    [SerializeField] private GameObject m_pausedMenu;
    
    //# 수정 사항(20250502) -- 시작
    [SerializeField] private GameObject m_gameStartPanel;
    public static GameObject GameStartPanel;
    //# SDW 수정 사항(20250502) -- 끝
    
    private static GameObject m_overPanel;
    
    [Header("Basic Setting")]
    // PlayerController
    public static CharacterController PlayerCont;
    // Player 위치
    public static Transform PlayerTransform;

    private void Awake()
    {
        PlayerCont = GetComponent<CharacterController>();
        PlayerTransform = transform;
        
        GameStartPanel = m_gameStartPanel;

        // GameOverPanel = GameOverPanel.GetComponent("Game Over Dialog Panel Dialog");GetComponent<Game Over Dialog Panel Dialog>();
        m_overPanel = GameObject.Find("Game Over Dialog Panel Dialog");
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
            if (GameStartPanel != null && GameStartPanel.activeSelf == true)
                return;
            
            if (GameManager.Instance.IsPaused == false)
            {
                m_pausedMenu.SetActive(true);
            }
            else
            {
                m_pausedMenu.SetActive(false);
                m_pausedMenu.GetComponent<PauseMenuUI>().Close();
            }
        }
    } 
    //# 수정 사항(20250502) -- 끝

    public static void Die()
    {
        m_overPanel.SetActive(true);
    }
}
