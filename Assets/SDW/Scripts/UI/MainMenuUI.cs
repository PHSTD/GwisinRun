using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button m_exitButton;

    
    void Start()
    {
        m_exitButton.onClick.AddListener(GameManager.Instance.Exit);
    }
}
