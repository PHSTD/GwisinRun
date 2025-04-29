using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //# 싱글턴
    private static GameManager m_instance;
    public static GameManager Instance => m_instance;
    
    public InputManager Input;
    public AudioSetting Audio;
    public GraphicsSetting Graphics;
    //todo 추후 리팩토링 시 검토
    // public UIManager UI;

    //todo 추후 사용 고민 게임 Time Attack에 사용할 Event
    // [NonSerialized] public UnityEvent OnGameStart;
    // [NonSerialized] public UnityEvent OnGameOver;
    [NonSerialized] public UnityEvent OnTimeChanged;

    //# 게임 Time Attack에 사용할 필드와 프로퍼티
    private float m_previousTime;
    public float PreviousTime => m_previousTime;

    private float m_currentTime;
    public float CurrentTime => m_currentTime;
    private float m_bestTime;
    public float BestTime => m_bestTime;

    //# GameOver 관련 필드와 프로퍼티
    private bool m_isGameOver;
    public bool IsGameOver => m_isGameOver;
    private bool m_isPaused;
    public bool IsPaused => m_isPaused;
    private bool m_isCleared;
    public bool IsCleared => m_isCleared;


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
        // OnGameOver = new UnityEvent();
        // OnGameOver.AddListener(GameOver);
        // OnGameStart = new UnityEvent();
        OnTimeChanged = new UnityEvent();
    }

    private void OnDisable()
    {
        // OnGameStart.RemoveAllListeners();
        // OnGameOver.RemoveAllListeners();
        OnTimeChanged.RemoveAllListeners();
    }

    private void Update()
    {
        if (!m_isGameOver && !m_isPaused && !m_isCleared)
        {
            m_currentTime += Time.deltaTime;
            if (m_currentTime - m_previousTime >= 0.1f)
            {
                OnTimeChanged?.Invoke();
                m_previousTime = m_currentTime;
            }
        }
    }

    public void GameStart()
    {
        m_currentTime = 0;
        m_previousTime = 0;
        m_isGameOver = false;
        // OnGameStart?.Invoke();
    }

    //todo GameOver는 Level에서 호출
    private void GameOver()
    {
        m_isGameOver = true;
    }

    //todo GameClear는 Level에서 호출
    public void GameClear(string sceneName)
    {
        if (m_currentTime < m_bestTime)
        {
            //todo PlayerPrefs 에 best score를 맵별로 저장해서 활용?
            m_bestTime = m_currentTime;
            
            //# Write
            PlayerPrefs.SetFloat(sceneName, m_bestTime);
        }
    }
    
    //todo GameRestart는 Level에서 호출
    public void GameRestart(string sceneName)
    { 
        //# Read
        m_bestTime= PlayerPrefs.GetFloat(sceneName);
        
        GameStart();
    }

    public void SceneLoader(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}