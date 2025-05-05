using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatioUI : MonoBehaviour
{
    private Button[] m_buttons;
    void Start()
    {
        m_buttons = GetComponentsInChildren<Button>(true);

        foreach (var button in m_buttons)
        {
            button.onClick.AddListener(() => GameManager.Instance.Audio.PlayClickSound());
        }

    }

    private void OnDisable()
    {
        foreach (var button in m_buttons)
        {
            button.onClick.RemoveListener(() => GameManager.Instance.Audio.PlayClickSound());
        }
    }
}
