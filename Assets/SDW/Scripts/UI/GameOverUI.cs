using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Restart")]
    [SerializeField] private Button m_restartButton;
    [SerializeField] private string m_levelSceneName;

    [Header("Main Menu")]
    [SerializeField] private Button m_mainMenuButton;
    [SerializeField] private string m_titleSceneName;
    
    [Header("Exit")]
    [SerializeField] private Button m_exitButton;
    
    [Header("Background")]
    [SerializeField] private Image m_titleBackground;
    [SerializeField] private Image m_blackBackground;
    
    private void OnEnable()
    {
        m_titleBackground.gameObject.SetActive(false);
        m_blackBackground.gameObject.SetActive(false);
        m_exitButton.onClick.AddListener(GameManager.Instance.Exit);
        m_mainMenuButton.onClick.AddListener(() => GameManager.Instance.SceneLoader(m_titleSceneName));
        m_restartButton.onClick.AddListener(() => GameManager.Instance.SceneLoader(m_levelSceneName));
    }
}
