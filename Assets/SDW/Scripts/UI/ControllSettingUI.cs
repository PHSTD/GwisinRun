using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class ControllSettingUI : MonoBehaviour
{
    [SerializeField] private Slider m_mouseSensitivitySlider;
    [SerializeField] private TMP_Text m_mouseSensitivityValue;
    
    private void OnEnable()
    {
        m_mouseSensitivitySlider.onValueChanged.AddListener(GameManager.Instance.Input.SetMouseSensitivity);
        m_mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivityText);
        m_mouseSensitivitySlider.value = GameManager.Instance.Input.MouseSensitivity;
    }

    private void OnDisable()
    {
        m_mouseSensitivitySlider.onValueChanged.RemoveListener(GameManager.Instance.Input.SetMouseSensitivity);
    }

    private void SetMouseSensitivityText(float value)
    {
        m_mouseSensitivityValue.text = value.ToString("0.0");
    }
}
