using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    [Header("Speed Settings")]
    // 걷기 속도
    public static float m_walkSpeed = 5f;
    // 뛰기 속도
    private float m_runSpeed = 10f;
    // 현재 속도를 걷기나 뛰기 속도를 저장
    public static float m_speed;
    // 떨어지는 속도
    private float m_fallSpeed = -28.0f;
    // 전체적인 이동 속도 계산
    private Vector3 m_velocity;
    
    [Header("Player Sit Setting")]
    private float m_sitHeight = 1.0f;
    // 캐릭터 원래 높이
    private float m_originalHeight;
    // 캐릭터 원래 중심 위치
    private Vector3 m_originalCenter;
    // 프레이어 크기 저장
    private Vector3 m_originalPlayerScale;
    // 앉은 상태의 스케일
    private Vector3 m_sitPlayerScale;
    // 앉은 상태인지 확인
    private bool m_isSit;
    // 앉기 키 누른 상태인지 확인
    private bool m_releasedSitKey;

    [Header("Jump Settings")]
    // 점프 높이
    private float m_jumpHeight = 1.2f;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_originalHeight = PlayerController.PlayerCont.height;
        m_originalCenter = PlayerController.PlayerCont.center;

        m_originalPlayerScale = PlayerController.PlayerTransform.localScale;
        m_sitPlayerScale = new Vector3(m_originalPlayerScale.x, m_originalPlayerScale.y * 0.5f, m_originalPlayerScale.z);

    }

    void Update()
    {
        Move();
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
            m_speed = m_runSpeed;
            if (moveX != 0 || moveZ != 0) PlayerHealth.StaminaMinus();
            else PlayerHealth.StaminaPlus();
        }
        else
        {
            m_speed = m_walkSpeed;
            PlayerHealth.StaminaPlus();
        }

        PlayerController.PlayerCont.Move( m_speed * Time.deltaTime * move);

        m_velocity.y += m_fallSpeed * Time.deltaTime;
        PlayerController.PlayerCont.Move(m_velocity * Time.deltaTime);

        if (PlayerController.PlayerCont.isGrounded && m_velocity.y < 0)
            m_velocity.y = -2f;
        
        
        //# 첫 움직임이 시작되면 GameManager의 Pause가 풀림
        if (PlayerController.PlayerRigidbody.velocity.magnitude != 0 && PlayerController.IsFirstMove)
        {
            GameManager.Instance.IsPaused = false;
            PlayerController.IsFirstMove = false;
        }
    }

    void Jump()
    {
        if (GameManager.Instance.Input.JumpKeyPressed && PlayerController.PlayerCont.isGrounded)
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
        
        if (sitKeyHeld || (m_isSit && PlayerController.HeadTriggerObject.IsDetected))
        {
            DoSit();
        }
        //# 키가 때졌을 때 또는 headTriggerObject의 감지가 안될 떄
        else if(m_releasedSitKey && !PlayerController.HeadTriggerObject.IsDetected)
        {
            Stand();
        }
    }

    void DoSit()
    {
        m_isSit = true;
        PlayerController.PlayerCont.height = m_sitHeight;
        PlayerController.PlayerCont.center = new Vector3(0, m_sitHeight / 2f, 0);
        PlayerController.PlayerTransform.localScale = m_sitPlayerScale;
    }

    void Stand()
    {
        // 바닥 레이어 감지용 Raycast
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        float rayDistance = 2f;

        bool hasGround = Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance, LayerMask.GetMask("Floor"));

        PlayerController.PlayerCont.enabled = false;

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
        PlayerController.PlayerCont.height = m_originalHeight;
        PlayerController.PlayerCont.center = m_originalCenter;
        PlayerController.PlayerTransform.localScale = m_originalPlayerScale;
        

        PlayerController.PlayerCont.enabled = true;
        m_isSit = false;
        m_releasedSitKey = false;
    }

}
