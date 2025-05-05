using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUI : MonoBehaviour
{
    [SerializeField] private GameObject m_inventoryContainer;
    [SerializeField] private GameObject m_currentTimeContainer;
    [SerializeField] private Button m_mainMenuButton;
    [SerializeField] private string m_sceneName;
    
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        m_inventoryContainer.SetActive(false);
        m_currentTimeContainer.SetActive(false);
        m_mainMenuButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(m_sceneName));
    }


    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.IsPaused = false;
        GameManager.Instance.IsGameOver = false;
        m_inventoryContainer.SetActive(true);
        m_currentTimeContainer.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        GameManager.Instance.ChangeScene(sceneName);
    }
}
