using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSetting : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer m_audioMixer;
    
    [Header("Audio Source")]
    [SerializeField] private AudioSource m_BGMSource;
    [SerializeField] private AudioSource m_SFXSource;

    [Header("Audio Clip")]
    public AudioClip TitleBackground;
    public AudioClip SceneBackground;
    
    public AudioClip Click;
    public AudioClip IdleSound;
    public AudioClip WalkSound;
    public AudioClip RunSound;

    private AudioModel m_initVolume = new AudioModel(0.5f, 0.5f);
    private AudioModel m_currentVolumeDB;
    private bool m_canPlay = true;

    private void Start()
    {
        VolumeInit();
        m_BGMSource.clip = TitleBackground;
        m_BGMSource.Play();
    }

    public void ChangeToTitleScene()
    {
        m_BGMSource.Stop();
        m_BGMSource.clip = TitleBackground;
        m_BGMSource.Play();
    }

    public void ChangeToGameScene()
    {
        m_BGMSource.Stop();
        m_BGMSource.clip = SceneBackground;
        m_BGMSource.Play();
    }
    
    private void VolumeInit()
    {
        SetBGMVolume(m_initVolume.BGMVolume);
        SetSFXVolume(m_initVolume.SFXVolume);
        m_currentVolumeDB.BGMVolume = CalculateVolumeToDB(m_initVolume.BGMVolume);
        m_currentVolumeDB.SFXVolume = CalculateVolumeToDB(m_initVolume.SFXVolume);
    }
    
    private float CalculateVolumeToDB(float volume)
    {
        float dB;
        if (volume <= 0.001f)
            dB = -80f;
        else
            dB = Mathf.Log10(volume) * 20f;
        return dB;
    }

    private float CalculateDBToVolume(float dB)
    {
        float volume = Mathf.Pow(10, dB/ 20);
        //# 소수점 둘째 자리까지 반올림
        return Mathf.Round(volume * 100f) / 100f;
    }
    
    public float SetBGMVolume(float volume)
    {
        float dB = CalculateVolumeToDB(volume);
        m_audioMixer.SetFloat("BGM", dB);
        return CalculateDBToVolume(dB);
    }

    public float SetSFXVolume(float volume)
    {
        float dB = CalculateVolumeToDB(volume);
        m_audioMixer.SetFloat("SFX", dB);
        return CalculateDBToVolume(dB);
    }

    public float GetBGMVolume()
    {
        return  CalculateDBToVolume(m_currentVolumeDB.BGMVolume);
    }

    public float GetSFXVolume()
    {
        return CalculateDBToVolume(m_currentVolumeDB.SFXVolume);
    }

    public void ApplyVolume()
    {
        m_audioMixer.GetFloat("BGM", out m_currentVolumeDB.BGMVolume);
        m_audioMixer.GetFloat("SFX", out m_currentVolumeDB.SFXVolume);
    }

    public void RevertVolume()
    {
        m_audioMixer.SetFloat("BGM", m_currentVolumeDB.BGMVolume);
        m_audioMixer.SetFloat("SFX", m_currentVolumeDB.SFXVolume);
    }

    public void ResetVolume()
    {
        VolumeInit();
    }

    public void PlaySFX(AudioClip clip)
    {
        m_SFXSource.PlayOneShot(clip);
    }

    public void PlayMoveSound(PlayerMoveState state)
    {
        if (m_SFXSource.isPlaying || !m_canPlay)
            return;

        float delayTime = 0f;
        
        switch (state)
        {
            case PlayerMoveState.Idle:
                m_SFXSource.clip = IdleSound;
                delayTime = 0.8f;
                break;
            case PlayerMoveState.Walk:
                m_SFXSource.clip = WalkSound;
                delayTime = 0.6f;
                break;
            case PlayerMoveState.Run:
                m_SFXSource.clip = RunSound;
                delayTime = 0.35f;
                break;
        }
        m_SFXSource.Play();
        m_canPlay = false;
        StartCoroutine(PlayMoveSoundStopDeleay(delayTime));
    }
    
    IEnumerator PlayMoveSoundStopDeleay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        m_SFXSource.Stop();
        m_canPlay = true;
    }
}
