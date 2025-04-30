using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameClearTrigger : MonoBehaviour
{
    [SerializeField] private GameObject m_gameClearPanel;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_gameClearPanel.SetActive(true);
            
        }
    }
}
