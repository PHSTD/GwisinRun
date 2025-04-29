using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GraphicsSetting : MonoBehaviour
{
    [Header("Graphics Setting")]
    [SerializeField] private PostProcessProfile m_brightness;
    [SerializeField] private PostProcessLayer m_postProcessLayer;
    
    private GraphicsModel m_defaultSetting;
    public GraphicsModel DefaultSetting => m_defaultSetting;
    private GraphicsModel m_setting;
    
    private AutoExposure exposure;
    
    private Resolution[] m_resolutions;

    private void Awake()
    {
        m_defaultSetting = new GraphicsModel()
        {
            Brightness = 1f,
            Quaility = 1,
            IsFullScreen = Screen.fullScreen
        };
    }

    private void Start()
    {
        m_brightness.TryGetSettings(out exposure);

        m_postProcessLayer = Camera.main.GetComponent<PostProcessLayer>();

        SetBrightness(exposure.keyValue.value);
    }
    
    
    //! Graphics Setting Dialog
    public void SetBrightness(float brightness)
    {
        if (brightness != 0)
        {
            exposure.keyValue.value = brightness;
        }
        else
        {
            exposure.keyValue.value = 0.1f;
        }
        m_setting.Brightness= brightness;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        m_setting.IsFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        m_setting.Quaility = qualityIndex;
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = m_resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void Reset()
    {
        QualitySettings.SetQualityLevel(m_defaultSetting.Quaility);
        Screen.fullScreen = m_defaultSetting.IsFullScreen;

        var currentResolution = Screen.currentResolution;
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
    }

    public void Apply()
    {
        QualitySettings.SetQualityLevel(m_setting.Quaility);
        Screen.fullScreen = m_setting.IsFullScreen;
    }


    public List<string> GetResolution(out int index)
    {
        m_resolutions = Screen.resolutions;

        var options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < m_resolutions.Length; i++)
        {
            string option = $"{m_resolutions[i].width} x {m_resolutions[i].height} {m_resolutions[i].refreshRateRatio}Hz";

            options.Add(option);

            if (m_resolutions[i].width == Screen.width && m_resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        index = currentResolutionIndex;
        return options;
    }
}