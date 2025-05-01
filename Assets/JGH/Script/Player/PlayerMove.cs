using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float m_walkSpeed = 5f;
    [SerializeField] private float m_runSpeed = 10f;
    [SerializeField] private float m_gravity = -9.81f;
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private CharacterController m_controller;
    [SerializeField] private Vector3 m_velocity;
    [SerializeField] private float m_zoomFOV = 30f;
    [SerializeField] private float m_normalFOV = 60f;
    [SerializeField] private float m_zoomSpeed = 10f;
    [SerializeField] private int m_stamina = 100;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_jumpHeight = 1.2f;

    private Camera m_playerCamera;
    private float m_timer = 0f;

    [SerializeField] private bool m_isCrouching = false;
    [SerializeField] private float m_originalHeight;
    [SerializeField] private Vector3 m_originalCenter;
    [SerializeField] private float m_crouchHeight = 1.0f;
    [SerializeField] private Transform m_capsuleModel;
    [SerializeField] private LayerMask m_ceilingMask; // 천장 레이어

    private Vector3 m_originalScale;
    private Vector3 m_crouchScale;

    private bool m_forceCrouch = false;
    private float m_headCheckDistance = 1.0f;

    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        m_playerCamera = m_cameraTransform.GetComponent<Camera>();
        m_playerCamera.fieldOfView = m_normalFOV;

        m_originalHeight = m_controller.height;
        m_originalCenter = m_controller.center;

        m_originalScale = m_capsuleModel.localScale;
        m_crouchScale = new Vector3(m_originalScale.x, m_originalScale.y * 0.5f, m_originalScale.z);
    }

    void Update()
    {
        Move();
        RotateView();
        ZoomView();
        Jump();
        HandleCrouch();
    }

    void Move()
    {
        Vector2 moveInput = GameManager.Instance.Input.MoveInput;
        float moveX = moveInput.x;
        float moveZ = moveInput.y;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (GameManager.Instance.Input.RunKeyBeingHeld)
        {
            m_speed = m_runSpeed;
            if (moveX != 0 || moveZ != 0) StaminaMinus();
            else StaminaPlus();
        }
        else
        {
            m_speed = m_walkSpeed;
            StaminaPlus();
        }

        m_controller.Move(move * m_speed * Time.deltaTime);

        m_velocity.y += m_gravity * Time.deltaTime;
        m_controller.Move(m_velocity * Time.deltaTime);

        if (m_controller.isGrounded && m_velocity.y < 0)
            m_velocity.y = -2f;
    }

    void Jump()
    {
        if (GameManager.Instance.Input.JumpKeyPressed && m_controller.isGrounded && !m_isCrouching)
        {
            m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
        }
    }

    void HandleCrouch()
    {
        bool crouchKeyHeld = GameManager.Instance.Input.SitKeyBeingHeld;
        bool ceilingBlocked = IsCeilingAbove();

        if (ceilingBlocked)
        {
            ForceCrouch();
            m_forceCrouch = true;
            return;
        }

        if (!crouchKeyHeld)
        {
            if (!m_forceCrouch && !IsCeilingAbove())
            {
                Stand();
            }
            return;
        }

        if (crouchKeyHeld)
        {
            Crouch();
            m_forceCrouch = false;
        }
    }

    bool IsCeilingAbove()
    {
        Vector3 origin = transform.position + Vector3.up * (m_controller.height / 2f);
        return Physics.Raycast(origin, Vector3.up, m_headCheckDistance, m_ceilingMask);
    }

    void Crouch()
    {
        if (m_isCrouching) return;

        m_controller.height = m_crouchHeight;
        m_controller.center = new Vector3(0, m_crouchHeight / 2f, 0);
        m_capsuleModel.localScale = m_crouchScale;
        m_isCrouching = true;
    }

    void Stand()
    {
        if (!m_isCrouching) return;
        if (IsCeilingAbove()) return;

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
        m_capsuleModel.localScale = m_originalScale;
        m_isCrouching = false;

        m_controller.enabled = true;
    }

    void ForceCrouch()
    {
        m_controller.height = m_crouchHeight;
        m_controller.center = new Vector3(0, m_crouchHeight / 2f, 0);
        m_capsuleModel.localScale = m_crouchScale;
        m_isCrouching = true;
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

        transform.Rotate(Vector3.up * mouseX * 2f);

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
