using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentTimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_currentTime;

    private void OnEnable()
    {
        m_currentTime.text = $"{GameManager.Instance.CurrentTime,6:F1}";
        GameManager.Instance.OnTimeChanged.AddListener(OnCurrentTimeChanged);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnTimeChanged.RemoveListener(OnCurrentTimeChanged);
    }

    private void OnCurrentTimeChanged()
    {
        m_currentTime.text = $"{GameManager.Instance.CurrentTime,6:F1}";
    }
}
