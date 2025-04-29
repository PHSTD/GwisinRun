using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button m_exitButton;
    [SerializeField] private Button m_mainMenuButton;

    [Header("Title Scene Name")]
    [SerializeField] private string m_sceneName;
    
    void Start()
    {
        m_exitButton.onClick.AddListener(GameManager.Instance.Exit);
        m_mainMenuButton.onClick.AddListener(() => GameManager.Instance.SceneLoader(m_sceneName));
    }
}
