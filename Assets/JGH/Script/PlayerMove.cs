using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float m_walkSpeed = 5f;        // 걷는 속도
    [SerializeField]
    private float m_runSpeed = 10f;        // 달리는 속도
    [SerializeField]
    private float m_gravity = -9.81f;      // 중력
    [SerializeField]
    private Transform m_cameraTransform;   // 1인칭 카메라

    [SerializeField]
    private CharacterController m_controller;
    [SerializeField]
    private Vector3 m_velocity;
    
    [SerializeField]
    private float m_zoomFOV = 30f;      // 최대 줌 인(좁은 시야각)
    [SerializeField]
    private float m_normalFOV = 60f;    // 기본 시야각
    [SerializeField]
    private float m_zoomSpeed = 10f;    // 줌 속도
    
    [SerializeField]
    private int m_stamina = 100;    // 스태미너

    private Camera m_playerCamera;      // 카메라 컴포넌트
    
    private float timer = 0f;
    
    [SerializeField]
    private float speed;
    
    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
        
        m_playerCamera = m_cameraTransform.GetComponent<Camera>();
        m_playerCamera.fieldOfView = m_normalFOV; // 처음 시야각
    }

    void Update()
    {
        Move();
        RotateView();
        ZoomView();
    }

    void Move()
    {
        Vector2 m_moveInput = GameManager.Instance.Input.MoveInput;
        float moveX = m_moveInput.x;
        float moveZ = m_moveInput.y;
        

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        
        
        // 달리기 - Shift키 누르면 m_runSpeed 적용
        if (GameManager.Instance.Input.RunKeyBeingHeld)
        {
           speed = m_runSpeed;
           if ((GameManager.Instance.Input.MoveInput.x != 0 || GameManager.Instance.Input.MoveInput.y != 0))
           {
               StaminaMinus();
           }
           else
           {
               StaminaPlus();
           }
        }
        // 걷기
        else
        {
           speed = m_walkSpeed;
           
           StaminaPlus();
        }
        
        m_controller.Move(move * speed * Time.deltaTime);

        // 중력 적용
        m_velocity.y += m_gravity * Time.deltaTime;
        m_controller.Move(m_velocity * Time.deltaTime);

        // 바닥에 있으면 중력 초기화
        if (m_controller.isGrounded && m_velocity.y < 0)
            m_velocity.y = -2f;
    }
    
    void StaminaPlus()
    {
           timer += Time.deltaTime; // 프레임마다 시간 누적
           if (timer >= 0.05f)
           {
               m_stamina++;
               timer = 0f;
           }
           
           if (m_stamina >= 100)
           {
               m_stamina = 100;
           }
    }

    void StaminaMinus()
    {
           timer += Time.deltaTime; // 프레임마다 시간 누적
           if (timer >= 0.05f)
           {
               m_stamina--;
               timer = 0f;
           }
           
           if (m_stamina <= 0)
           {
               m_stamina = 0;
               speed = m_walkSpeed;
           }
    }

    void RotateView()
    {
        // TODO: 마우스 기능 추가시 수정 필요
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 좌우 회전 (플레이어 전체)
        transform.Rotate(Vector3.up * mouseX * 2f);

        // 상하 회전 (카메라만)
        float rotationX = m_cameraTransform.localEulerAngles.x - mouseY * 2f;
        if (rotationX > 180) rotationX -= 360;
        rotationX = Mathf.Clamp(rotationX, -80, 80);
        m_cameraTransform.localEulerAngles = new Vector3(rotationX, 0, 0);
    }
    
    void ZoomView()
    {
        // TODO: 마우스 기능 추가시 수정 필요
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // 마우스 휠 위/아래로 줌 인/아웃
            m_normalFOV -= scroll * m_zoomSpeed;
            m_normalFOV = Mathf.Clamp(m_normalFOV, m_zoomFOV, 60f); // 60은 기본값, 조정 가능
        }
        m_playerCamera.fieldOfView = Mathf.Lerp(m_playerCamera.fieldOfView, m_normalFOV, Time.deltaTime * 10f);
    }
}