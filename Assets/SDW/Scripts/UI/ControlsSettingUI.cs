using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsSettingUI : MonoBehaviour
{
    [SerializeField] private Button m_backButton;
    
    void Start()
    {
        // m_backButton.onClick.AddListener(GameManager.Instance.Input.SaveRebindingKeys);
    }
}
