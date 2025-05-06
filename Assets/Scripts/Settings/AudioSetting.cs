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
    [SerializeField] private AudioSource m_heartBeatSource;
    [SerializeField] private AudioSource m_SFXSource;

    [Header("Audio Clip")]
    public AudioClip TitleBackground;
    public AudioClip SceneBackground;
    
    public AudioClip Click;
    public AudioClip IdleSound;
    public AudioClip WalkSound;
    public AudioClip RunSound;
    public AudioClip PotionSound;
    public AudioClip GhostAttackSound;
    public AudioClip SwitchSound;
    public AudioClip LockedDoorSound;
    public AudioClip GetItemSound;
    public AudioClip DropItemSound;
    public AudioClip DoorOpenSound;
    public AudioClip DoorCloseSound;
    public AudioClip DrawerOpenSound;
    public AudioClip DrawerCloseSound;

    private AudioModel m_initVolume = new AudioModel(0.5f, 0.5f);
    private AudioModel m_currentVolumeDB;
    private bool m_canPlay = true;
    private bool m_isGhostSoundPlaying;

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
        m_audioMixer.SetFloat("HeartBeat", dB);
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
        m_audioMixer.SetFloat("HeartBeat", m_currentVolumeDB.SFXVolume);
    }

    public void ResetVolume()
    {
        VolumeInit();
    }

    public void PlayClickSound()
    {
        m_SFXSource.PlayOneShot(Click);
    }

    public void PlayMoveSound(PlayerMoveState state)
    {
        if (m_heartBeatSource.isPlaying || !m_canPlay)
            return;

        float delayTime = 0f;
        
        switch (state)
        {
            case PlayerMoveState.Idle:
                m_heartBeatSource.clip = IdleSound;
                delayTime = 0.8f;
                break;
            case PlayerMoveState.Walk:
                m_heartBeatSource.clip = WalkSound;
                delayTime = 0.6f;
                break;
            case PlayerMoveState.Run:
                m_heartBeatSource.clip = RunSound;
                delayTime = 0.35f;
                break;
        }
        m_heartBeatSource.Play();
        m_canPlay = false;
        StartCoroutine(PlayMoveSoundStopDelay(delayTime));
    }
    
    IEnumerator PlayMoveSoundStopDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        m_heartBeatSource.Stop();
        m_canPlay = true;
    }

    public void PlaySound(SoundType type)
    {
        switch (type)
        {
            case SoundType.Potion:
                m_SFXSource.PlayOneShot(PotionSound);
                break;
            case SoundType.GhostAttack:
                if (!m_isGhostSoundPlaying)
                    StartCoroutine(PlayGhostSound());
                break;
            case SoundType.Switch:
                m_SFXSource.PlayOneShot(SwitchSound);
                break;
            case SoundType.LockedDoor:
                m_SFXSource.PlayOneShot(LockedDoorSound);
                break;
            case SoundType.GetItem:
                m_SFXSource.PlayOneShot(GetItemSound);
                break;
            case SoundType.DropItem:
                m_SFXSource.PlayOneShot(DropItemSound);
                break;
            case SoundType.DoorOpen:
                m_SFXSource.PlayOneShot(DoorOpenSound);
                break;
            case SoundType.DoorClose:
                m_SFXSource.PlayOneShot(DoorCloseSound);
                break;
            case SoundType.DrawerOpen:
                m_SFXSource.PlayOneShot(DrawerOpenSound);
                break;
            case SoundType.DrawerClose:
                // m_SFXSource.PlayOneShot(DrawerOpenSound);
                m_SFXSource.PlayOneShot(DrawerCloseSound);
                break;
        }
    }

    IEnumerator PlayGhostSound()
    {
        m_SFXSource.PlayOneShot(GhostAttackSound);
        m_isGhostSoundPlaying = true;
        yield return new WaitForSeconds(GhostAttackSound.length + 0.2f);
        m_isGhostSoundPlaying = false;
    }
}
