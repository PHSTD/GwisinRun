using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float m_walkSpeed = 5f;
    [SerializeField] private float m_runSpeed = 10f;
    [SerializeField] private float m_fallSpeed = -9.81f;
    private Vector3 m_velocity;
    
    [Header("Player Sight Settings")]
    [SerializeField] private float m_zoomFOV = 30f;
    [SerializeField] private float m_normalFOV = 60f;
    [SerializeField] private float m_zoomSpeed = 10f;
    
    [Header("Player Sit Setting")]
    [SerializeField] private float m_sitHeight = 1.0f;
    private float m_originalHeight;
    private Vector3 m_originalCenter;
    private Vector3 m_originalPlayerScale;
    private Vector3 m_sitPlayerScale;
    private bool m_isSit;
    private bool m_releasedSitKey;
    
    [Header("Stamina & Jump Settings")]
    [SerializeField] private int m_stamina = 100;
    [SerializeField] private float m_jumpHeight = 1.2f;
    
    [Header("Basic Setting")]
    [SerializeField] private CharacterController m_controller;
    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private PlayerHide m_headTriggerObject;
    
    [Header("Camera Settings")]
    [SerializeField] private Transform m_cameraTransform;
    private Camera m_playerCamera;
    
    private float m_timer = 0f;
    private float m_speed;

    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        m_playerCamera = m_cameraTransform.GetComponent<Camera>();
        m_playerCamera.fieldOfView = m_normalFOV;

        m_originalHeight = m_controller.height;
        m_originalCenter = m_controller.center;

        m_originalPlayerScale = m_playerTransform.localScale;
        m_sitPlayerScale = new Vector3(m_originalPlayerScale.x, m_originalPlayerScale.y * 0.5f, m_originalPlayerScale.z);
    }

    void Update()
    {
        Move();
        RotateView();
        ZoomView();
        Jump();
        SitPlayer();
    }

    void Move()
    {
        Vector2 moveInput = GameManager.Instance.Input.MoveInput;
        float moveX = moveInput.x;
        float moveZ = moveInput.y;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (GameManager.Instance.Input.RunKeyBeingHeld)
        {
            //todo 스태미너 100일 경우 -> 예를 들어 1이 이깍히면 1%(맥스)
            /*
             * todo runSpeed의 minimum : walkspeed
             * runSpeed의 1%(맥스)
            */
            m_speed = m_runSpeed;
            if (moveX != 0 || moveZ != 0) StaminaMinus();
            else StaminaPlus();
        }
        else
        {
            m_speed = m_walkSpeed;
            StaminaPlus();
        }

        m_controller.Move( m_speed * Time.deltaTime * move);

        m_velocity.y += m_fallSpeed * Time.deltaTime;
        m_controller.Move(m_velocity * Time.deltaTime);

        if (m_controller.isGrounded && m_velocity.y < 0)
            m_velocity.y = -2f;
    }

    void Jump()
    {
        if (GameManager.Instance.Input.JumpKeyPressed && m_controller.isGrounded)
        {
            m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_fallSpeed);
        }
    }

    void SitPlayer()
    {
        bool sitKeyHeld = GameManager.Instance.Input.SitKeyBeingHeld;
        
        if(GameManager.Instance.Input.SitKeyReleased)
        {
            m_releasedSitKey = true;
        }
        
        if (sitKeyHeld || (m_isSit && m_headTriggerObject.IsDetected))
        {
            DoSit();
        }
        //# 키게 때졌을 때 또는 headTriggerObject의 감지가 안될 떄
        else if(m_releasedSitKey && !m_headTriggerObject.IsDetected)
        {
            Stand();
        }
    }

    void DoSit()
    {
        m_isSit = true;
        m_controller.height = m_sitHeight;
        m_controller.center = new Vector3(0, m_sitHeight / 2f, 0);
        m_playerTransform.localScale = m_sitPlayerScale;
    }

    void Stand()
    {
        // 바닥 레이어 감지용 Raycast
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        float rayDistance = 2f;

        bool hasGround = Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance, LayerMask.GetMask("Floor"));

        m_controller.enabled = false;

        // 바닥 없으면 위치 강제로 조정
        if (hasGround)
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.05f, transform.position.z);
        }
        else
        {
            // 최후 보정: 아주 살짝 위로 띄움
            transform.position += Vector3.up * 0.2f;
        }

        // 기본값 복구
        m_controller.height = m_originalHeight;
        m_controller.center = m_originalCenter;
        m_playerTransform.localScale = m_originalPlayerScale;

        m_controller.enabled = true;
        m_isSit = false;
        m_releasedSitKey = false;
    }

    void StaminaPlus()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= 0.05f)
        {
            m_stamina++;
            m_timer = 0f;
        }
        if (m_stamina >= 100) m_stamina = 100;
    }

    void StaminaMinus()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= 0.05f)
        {
            m_stamina--;
            m_timer = 0f;
        }
        if (m_stamina <= 0)
        {
            m_stamina = 0;
            m_speed = m_walkSpeed;
        }
    }

    void RotateView()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(mouseX * 2f * Vector3.up);

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
            m_normalFOV -= scroll * m_zoomSpeed;
            m_normalFOV = Mathf.Clamp(m_normalFOV, m_zoomFOV, 60f);
        }
        m_playerCamera.fieldOfView = Mathf.Lerp(m_playerCamera.fieldOfView, m_normalFOV, Time.deltaTime * 10f);
    }
}
