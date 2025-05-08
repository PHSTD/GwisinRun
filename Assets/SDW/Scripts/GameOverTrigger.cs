using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    [SerializeField] private GameObject m_gameOverPanel;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_gameOverPanel.SetActive(true);
        }
    }
}
