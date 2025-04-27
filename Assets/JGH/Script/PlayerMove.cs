using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 5f;        // 걷는 속도
    public float runSpeed = 10f;        // 달리는 속도
    public float gravity = -9.81f;      // 중력
    public Transform cameraTransform;   // 1인칭 카메라

    private CharacterController controller;
    private Vector3 velocity;
    
    public float zoomFOV = 30f;      // 최대 줌 인(좁은 시야각)
    public float normalFOV = 60f;    // 기본 시야각
    public float zoomSpeed = 10f;    // 줌 속도

    private Camera playerCamera;      // 카메라 컴포넌트

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
        
        playerCamera = cameraTransform.GetComponent<Camera>();
        playerCamera.fieldOfView = normalFOV; // 처음 시야각
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
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 바닥에 있으면 중력 초기화
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
    }

    void RotateView()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 좌우 회전 (플레이어 전체)
        transform.Rotate(Vector3.up * mouseX * 2f);

        // 상하 회전 (카메라만)
        float rotationX = cameraTransform.localEulerAngles.x - mouseY * 2f;
        if (rotationX > 180) rotationX -= 360;
        rotationX = Mathf.Clamp(rotationX, -80, 80);
        cameraTransform.localEulerAngles = new Vector3(rotationX, 0, 0);
    }
    
    void ZoomView()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // 마우스 휠 위/아래로 줌 인/아웃
            normalFOV -= scroll * zoomSpeed;
            normalFOV = Mathf.Clamp(normalFOV, zoomFOV, 60f); // 60은 기본값, 조정 가능
        }
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * 10f);
    }
}

