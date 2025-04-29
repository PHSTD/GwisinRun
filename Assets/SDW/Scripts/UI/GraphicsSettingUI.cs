using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingUI : MonoBehaviour
{
    [Header("Graphics UI Setting")]
    [SerializeField] private TMP_Text m_brightnessTextValue = null;
    [SerializeField] private Slider m_brightnessSlider = null;
    [SerializeField] private Toggle m_fullscreenToggle;
    [SerializeField] private TMP_Dropdown m_resolutionDropdown;
    [SerializeField] private TMP_Dropdown m_qualityDropdown;
    [SerializeField] private Button m_resetButton;
    [SerializeField] private Button m_applyButton;
    
    private Resolution[] m_resolutions;

    private void Start()
    {
        m_brightnessSlider.onValueChanged.AddListener(GameManager.Instance.Graphics.SetBrightness);
        m_brightnessSlider.onValueChanged.AddListener(GraphicsDialogSetBrightness);
        
        m_fullscreenToggle.onValueChanged.AddListener(GameManager.Instance.Graphics.SetFullScreen);
        
        m_resolutionDropdown.onValueChanged.AddListener(GameManager.Instance.Graphics.SetResolution);
        
        m_qualityDropdown.onValueChanged.AddListener(GameManager.Instance.Graphics.SetQuality);
        
        
        m_resetButton.onClick.AddListener(GameManager.Instance.Graphics.Reset);
        m_resetButton.onClick.AddListener(GraphicsDialogResetButton);
        
        m_applyButton.onClick.AddListener((GameManager.Instance.Graphics.Apply));
        

        InitUISetting();
    }

    private void InitUISetting()
    {
        GraphicsModel defaultSetting = GameManager.Instance.Graphics.DefaultSetting;
        m_brightnessSlider.value = defaultSetting.Brightness;
        m_brightnessTextValue.text = defaultSetting.Brightness.ToString("0,0");

        m_qualityDropdown.value = defaultSetting.Quaility;

        m_fullscreenToggle.isOn = defaultSetting.IsFullScreen;

        GetResolution();
    }

    private void GetResolution()
    {
        m_resolutionDropdown.ClearOptions();
        List<string> options = GameManager.Instance.Graphics.GetResolution(out int currentResolutionIndex);
        m_resolutionDropdown.AddOptions(options);
        m_resolutionDropdown.value = currentResolutionIndex;
        m_resolutionDropdown.RefreshShownValue();
    }

    private void GraphicsDialogSetBrightness(float brightness)
    {
        m_brightnessTextValue.text = (brightness * 20).ToString("0");
    }
    

    private void GraphicsDialogResetButton()
    {
        InitUISetting();
    }
}
