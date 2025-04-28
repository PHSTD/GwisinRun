using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //# 싱글턴
    private static GameManager m_instance;
    public static GameManager Instance => m_instance;
    
    public InputManager Input;
    public AudioSetting Audio;

    //# 게임 Time Attack에 사용할 Event
    [NonSerialized] public UnityEvent OnGameStart;
    [NonSerialized] public UnityEvent OnGameOver;
    [NonSerialized] public UnityEvent OnTimeChanged;

    //# 게임 Time Attack에 사용할 필드와 프로퍼티
    private float m_previousTime;
    public float PreviousTime => m_previousTime;

    private float m_currentTime;
    public float CurrentTime => m_currentTime;

    private float m_highestTime;
    public float HighestTime => m_highestTime;

    //# GameOver 관련 필드와 프로퍼티
    private bool m_isGameOver;
    public bool IsGameOver => m_isGameOver;

    private void Awake()
    {
    }

    public static void CreateInstance()
    {
        if (m_instance == null)
        {
            var gameManagerPrefab = Resources.Load<GameManager>("GameManager");
            m_instance = Instantiate(gameManagerPrefab);
            DontDestroyOnLoad(m_instance);
        }
    }

    public static void ReleaseInstance()
    {
        if (m_instance != null)
        {
            Destroy(m_instance);
            m_instance = null;
        }
    }

    private void OnEnable()
    {
        //# 현재는 GameOver만 등록
        OnGameOver = new UnityEvent();
        OnGameOver.AddListener(GameOver);
        OnGameStart = new UnityEvent();
        OnTimeChanged = new UnityEvent();
    }

    private void OnDisable()
    {
        OnGameStart.RemoveAllListeners();
        OnGameOver.RemoveAllListeners();
        OnTimeChanged.RemoveAllListeners();
    }

    private void Update()
    {
        //# 타입어택 적용 시 사용할 부분, 현재 주석 처리
        // if (!m_isGameOver)
        // {
        //     m_currentTime += Time.deltaTime;
        //     if (m_currentTime - m_previousTime >= 0.1f)
        //     {
        //         OnTimeChanged?.Invoke();
        //         m_previousTime = m_currentTime;
        //     }
        // }
    }

    public void GameStart()
    {
        m_currentTime = 0;
        m_previousTime = 0;
        m_isGameOver = false;
        OnGameStart?.Invoke();
    }

    private void GameOver()
    {
        if (m_currentTime > m_highestTime)
        {
            m_highestTime = m_currentTime;
        }
        m_isGameOver = true;
    }
}