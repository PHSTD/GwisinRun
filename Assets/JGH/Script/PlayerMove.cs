using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float WalkSpeed = 5f;        // 걷는 속도
    public float RunSpeed = 10f;        // 달리는 속도
    public float Gravity = -9.81f;      // 중력
    public Transform m_cameraTransform;   // 1인칭 카메라

    private CharacterController m_controller;
    private Vector3 m_velocity;
    
    public float ZoomFOV = 30f;      // 최대 줌 인(좁은 시야각)
    public float NormalFOV = 60f;    // 기본 시야각
    public float ZoomSpeed = 10f;    // 줌 속도

    private Camera m_playerCamera;      // 카메라 컴포넌트

    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
        
        m_playerCamera = m_cameraTransform.GetComponent<Camera>();
        m_playerCamera.fieldOfView = NormalFOV; // 처음 시야각
    }

    void Update()
    {
        Move();
        RotateView();
        ZoomView();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // 달리기 - Shift키 누르면 runSpeed 적용
        float speed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        m_controller.Move(move * speed * Time.deltaTime);

        // 중력 적용
        m_velocity.y += Gravity * Time.deltaTime;
        m_controller.Move(m_velocity * Time.deltaTime);

        // 바닥에 있으면 중력 초기화
        if (m_controller.isGrounded && m_velocity.y < 0)
            m_velocity.y = -2f;
    }

    void RotateView()
    {
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
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // 마우스 휠 위/아래로 줌 인/아웃
            NormalFOV -= scroll * ZoomSpeed;
            NormalFOV = Mathf.Clamp(NormalFOV, ZoomFOV, 60f); // 60은 기본값, 조정 가능
        }
        m_playerCamera.fieldOfView = Mathf.Lerp(m_playerCamera.fieldOfView, NormalFOV, Time.deltaTime * 10f);
    }
}

