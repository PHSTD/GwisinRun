using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button m_newGameYesButton;
    [SerializeField] private Button m_exitButton;

    [Header("New Game Scene Name")]
    [SerializeField] private string m_sceneName;
    
    void Start()
    {
        m_newGameYesButton.onClick.AddListener(() => GameManager.Instance.SceneLoader(m_sceneName));
        m_exitButton.onClick.AddListener(GameManager.Instance.Exit);
    }
}
