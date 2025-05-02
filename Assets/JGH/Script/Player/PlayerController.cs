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
    public static Rigidbody PlayerRigidbody;

    private void Awake()
    {
        // 인스펙터 창에서 넣어주기 위해 
        PausedMenu = m_pausedMenu;
        PlayerCont = GetComponent<CharacterController>();
        
        PlayerTransform = transform;
        HeadTriggerObject = GetComponentInChildren<PlayerHide>();
        
        PlayerRigidbody = GetComponent<Rigidbody>();
    }
}
