using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingUI : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text m_BGMTextValue = null;
    [SerializeField] private Slider m_BGMSlider = null;
    [SerializeField] private TMP_Text m_SFXTextValue = null;
    [SerializeField] private Slider m_SFXSlider = null;
    [SerializeField] private Button m_ResetButton = null;
    [SerializeField] private Button m_BackButton = null;
    [SerializeField] private Button m_ApplyButton = null;

    private void Start()
    {
        SetBGMSliderAndText();
        SetSFXSliderAndText();
        m_BGMSlider.onValueChanged.AddListener(SoundDialogSetBGMVolume);
        m_SFXSlider.onValueChanged.AddListener(SoundDialogSetSFXVolume);
        m_ResetButton.onClick.AddListener(SoundDialogResetButton);
        m_BackButton.onClick.AddListener(SoundDialogBackButton);
        m_ApplyButton.onClick.AddListener(SoundDialogApplyButton);
    }
    
    public void SoundDialogSetBGMVolume(float volume)
    {
        float newVolume = GameManager.Instance.Audio.SetBGMVolume(volume);
        m_BGMSlider.value = newVolume;
        m_BGMTextValue.text = $"{(int)(newVolume * 100),0}%";
    }

    public void SoundDialogSetSFXVolume(float volume)
    {
        float newVolume = GameManager.Instance.Audio.SetSFXVolume(volume);
        m_SFXSlider.value = newVolume;
        m_SFXTextValue.text = $"{(int)(newVolume * 100),0}%";
    }
    
    public void SoundDialogApplyButton()
    {
        GameManager.Instance.Audio.ApplyVolume();
    }

    public void SoundDialogBackButton()
    {
        GameManager.Instance.Audio.RevertVolume();
        SetBGMSliderAndText();
        SetSFXSliderAndText();
    }

    public void SoundDialogResetButton()
    {
        GameManager.Instance.Audio.ResetVolume();
        SetBGMSliderAndText();
        SetSFXSliderAndText();
    }


    public void SetBGMSliderAndText()
    {
        float BGMVolume = GameManager.Instance.Audio.GetBGMVolume();
        m_BGMSlider.value = BGMVolume;
        m_BGMTextValue.text = $"{(int)(BGMVolume * 100),0}%";
    }

    public void SetSFXSliderAndText()
    {
        float SFXVolume = GameManager.Instance.Audio.GetSFXVolume();
        m_SFXSlider.value = SFXVolume;
        m_SFXTextValue.text = $"{(int)(SFXVolume * 100),0}%";
    }

    public void PlayButton()
    {
        GameManager.Instance.Audio.PlaySFX(GameManager.Instance.Audio.Click);
    }

}
