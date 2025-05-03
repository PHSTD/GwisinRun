using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Game UI")]
    [SerializeField] private GameObject m_pausedMenu;
    
    //# 수정 사항(20250502) -- 시작
    [SerializeField] private GameObject m_gameStartPanel;
    public static GameObject GameStartPanel;
    //# SDW 수정 사항(20250502) -- 끝
    
    public GameObject GameOverPanel;
    
    [Header("Basic Setting")]
    // PlayerController
    public static CharacterController PlayerCont;
    // Player 위치
    public static Transform PlayerTransform;

    private void Awake()
    {
        PlayerCont = GetComponent<CharacterController>();
        PlayerTransform = transform;
    }

    private void Start()
    {
        GameStartPanel = m_gameStartPanel;
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

    public void Die()
    {
        GameOverPanel.SetActive(true);
    }
}
