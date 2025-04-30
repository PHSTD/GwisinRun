using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button m_exitButton;
    [SerializeField] private Button m_mainMenuButton;

    [Header("Title Scene Name")]
    [SerializeField] private string m_sceneName;

    [Header("Background")]
    [SerializeField] private Image m_titleBackground;
    [SerializeField] private Image m_blackBackground;

    [Header("Current Time in Game")]
    [SerializeField] private GameObject m_currentTimeContainer;
    
    private void OnEnable()
    {
        GameManager.Instance.IsPaused = true;
        
        m_titleBackground.gameObject.SetActive(true);
        m_blackBackground.gameObject.SetActive(true);
        m_currentTimeContainer.SetActive(false);
        
        m_exitButton.onClick.AddListener(GameManager.Instance.Exit);
        m_mainMenuButton.onClick.AddListener(() => GameManager.Instance.SceneLoader(m_sceneName));

    }

    public void Close()
    {
        m_exitButton.onClick.RemoveAllListeners();
        m_mainMenuButton.onClick.RemoveAllListeners();
        
        m_titleBackground.gameObject.SetActive(false);
        m_blackBackground.gameObject.SetActive(false);
        m_currentTimeContainer.SetActive(true);
        
        GameManager.Instance.IsPaused = false;
    }
}
