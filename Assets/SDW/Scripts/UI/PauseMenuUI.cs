using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
    
    private void OnEnable()
    {
        m_exitButton.onClick.AddListener(GameManager.Instance.Exit);
        m_mainMenuButton.onClick.AddListener(() => GameManager.Instance.SceneLoader(m_sceneName));
        
        m_titleBackground.gameObject.SetActive(true);
        m_blackBackground.gameObject.SetActive(true);
    }

    public void Close()
    {
        m_exitButton.onClick.RemoveAllListeners();
        m_mainMenuButton.onClick.RemoveAllListeners();
        
        m_titleBackground.gameObject.SetActive(false);
        m_blackBackground.gameObject.SetActive(false);
    }
}
