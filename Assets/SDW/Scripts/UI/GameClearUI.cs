using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameClearUI : MonoBehaviour
{
    [Header("Time Text")]
    [SerializeField] private TMP_Text m_clearTime;
    [SerializeField] private TMP_Text m_bestTime;

    [Header("Main Menu")]
    [SerializeField] private Button m_mainMenuButton;
    [SerializeField] private string m_titleSceneName;
    
    [Header("Restart")]
    [SerializeField] private Button m_restartButton;
    [SerializeField] private string m_levelSceneName;
    
    [Header("Next Level")]
    [SerializeField] private Button m_nextLevelButton;
    [SerializeField] private string m_nextSceneName;
    
    [Header("Background")]
    [SerializeField] private Image m_titleBackground;
    [SerializeField] private Image m_blackBackground;
    
    private void OnEnable()
    {
        //todo GameManager의 IsPaused를 true, IsGameOver, IsClear, IsPaused로 구분할지 고민
        //todo 해당 UI가 Enable되면, GameManager의 클리어 시간, 해당 레벨에서의 최단 시간 표시
        //todo 클리어 시간과 최단 시간이 같을 때(갱신) New 표시 여부 검토
        m_mainMenuButton.onClick.AddListener(() => GameManager.Instance.SceneLoader(m_titleSceneName));
        m_restartButton.onClick.AddListener(() => GameManager.Instance.SceneLoader(m_levelSceneName));
        m_nextLevelButton.onClick.AddListener(() => GameManager.Instance.SceneLoader(m_nextSceneName));
        
        m_titleBackground.gameObject.SetActive(false);
        m_blackBackground.gameObject.SetActive(false);
    }
}
