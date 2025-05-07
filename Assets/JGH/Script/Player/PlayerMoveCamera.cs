using System.Collections;
using UnityEngine;

public class PlayerMoveCamera : MonoBehaviour
{
    [Header("Player Sight Settings")]
    private float m_zoomFOV = 30f;
    private float m_normalFOV = 60f;
    private float m_zoomSpeed = 10f;
    
    [Header("Camera Settings")]
    private Transform m_cameraTransform;
    private Camera PlayerCamera;
    
    [Header("Head Bobbing")]
    private Vector3 m_cameraDefaultPos; // 카메라 위치 - 서있을떄
    private Vector3 m_cameraSitPos;  // 카메라 위치 - 앉았을때
    private float m_bobTimer = 0f;      // 흔들림 계산용 타이머
    private float m_bobFrequencyWalk = 6f;    // 걷기 시 위아래 흔들림 속도 (진동 주기)
    private float m_bobFrequencyRun = 10f;    // 뛰기 시 위아래 흔들림 속도
    private float m_bobAmplitudeWalk = 0.11f; // 걷기 시 위아래 흔들림 크기
    private float m_bobAmplitudeRun = 0.25f;  // 뛰기 시 위아래 흔들림 크기

    private PlayerHealth m_playerHealth;
    private PlayerController m_playerController;
    private PlayerMove m_playerMove;
    
    // 공격 받으면 공격한 몬스터한데 시선 고정
    private Coroutine m_focusRoutine;


    private void Start()
    {
        m_playerHealth = GetComponent<PlayerHealth>();
        m_playerController = GetComponent<PlayerController>();
        m_playerMove = GetComponent<PlayerMove>();
        
        // PlayerCamera = m_cameraTransform.GetComponent<Camera>();
        // 메인 카메라를 찾아 저장한다
        GameObject camObj = GameObject.FindWithTag("MainCamera");
        PlayerCamera = camObj.GetComponent<Camera>();
        m_cameraTransform = PlayerCamera.transform;
        
        // 고정 시야 높이 설정
        m_cameraDefaultPos = new Vector3(0, 1.20f, 0); // 서 있을 때
        m_cameraSitPos = new Vector3(0, 0.50f, 0);     // 앉았을 때

        // 초점 처리를 위해
        PlayerCamera.fieldOfView = m_normalFOV;
        
    }
    
    public void LookAtTemporarily(Transform target, float duration)
    {
        if (target == null) return;

        if (m_focusRoutine != null)
            StopCoroutine(m_focusRoutine);

        m_focusRoutine = StartCoroutine(FocusRoutine(target, duration));
    }
    
    private IEnumerator FocusRoutine(Transform target, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            if (target == null) yield break;

            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // 카메라 본체를 부드럽게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            timer += Time.deltaTime;
            yield return null;
        }

        Vector3 euler = transform.eulerAngles;
        euler.x = 0f;
        euler.z = 0f;

        transform.eulerAngles = euler;

        m_focusRoutine = null;
    }
    
    
    public void ShakeCamera(float duration = 0.3f, float magnitude = 0.2f)
    {
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }
    
    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        Vector3 originalPos = m_cameraTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            m_cameraTransform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        Vector3 euler = transform.eulerAngles;
        euler.x = 0f;
        euler.z = 0f;

        m_cameraTransform.localPosition = originalPos;
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
            return;
        
        RotateView();
        ZoomView();
        HeadBob(); // 캐릭터 이동 시 카메라 위아래 흔들림 적용
    }
    
    void ZoomView()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            m_normalFOV -= scroll * m_zoomSpeed;
            m_normalFOV = Mathf.Clamp(m_normalFOV, m_zoomFOV, 60f);
        }
        PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, m_normalFOV, Time.deltaTime * 10f);
    }
    
    void RotateView()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(mouseX * GameManager.Instance.Input.MouseSensitivity * Vector3.up);

        float rotationX = m_cameraTransform.localEulerAngles.x - mouseY * GameManager.Instance.Input.MouseSensitivity;
        if (rotationX > 180) rotationX -= 360;
        rotationX = Mathf.Clamp(rotationX, -80, 80);
        m_cameraTransform.localEulerAngles = new Vector3(rotationX, 0, 0);
    }

    /// <summary>
    /// 걷기/뛰기 시 카메라 위아래로 흔들리게 처리하는 함수
    /// </summary>
    void HeadBob()
    {
            // 현재 이동 입력이 있고, 땅에 닿아 있으며 앉아있지 않은 상태에서만 흔들림 적용
            Vector2 moveInput = GameManager.Instance.Input.MoveInput;
            bool isMoving = moveInput.sqrMagnitude > 0.01f && m_playerController.PlayerCont.isGrounded;
            // 기준 카메라 위치 결정
            Vector3 baseCamPos = m_playerMove.IsSit ? m_cameraSitPos : m_cameraDefaultPos;
            
            if (isMoving && !m_playerMove.IsSit)
            {
                // 걷기와 뛰기 여부에 따라 주기(frequency)와 진폭(amplitude) 선택
                float frequency;
                float amplitude;
                // 뛰기를 눌렀고 스태미너가 있으면 뛰기
                if (GameManager.Instance.Input.RunKeyBeingHeld && m_playerHealth.GetCurrentStamina() != 0)
                {
                    frequency = m_bobFrequencyRun;
                    amplitude = m_bobAmplitudeRun ;
                }
                // 뛰기를 눌렀는데 스태미너가 없으면 걷기로 강제 전환
                else
                {
                    frequency = m_bobFrequencyWalk;
                    amplitude = m_bobAmplitudeWalk ;
                }

                // 사인파를 기반으로 한 시간 계산
                m_bobTimer += Time.deltaTime * frequency;

                // sin 값으로 카메라의 y축 오프셋 계산
                float bobOffset = Mathf.Sin(m_bobTimer) * amplitude;

                // 원래 위치 + 위아래 흔들림 적용
                //m_cameraTransform.localPosition = m_cameraDefaultPos + new Vector3(0, bobOffset, 0);
                 m_cameraTransform.localPosition = Vector3.Lerp(
                m_cameraTransform.localPosition,
                baseCamPos + new Vector3(0, bobOffset, 0),
                Time.deltaTime * 5f
            );
        }
        else
        {
            // 이동하지 않을 경우 타이머 초기화 및 카메라 위치를 원래 위치로 부드럽게 되돌림
            m_bobTimer = 0f;
            m_cameraTransform.localPosition = Vector3.Lerp(m_cameraTransform.localPosition, baseCamPos, Time.deltaTime * 5f);
        }
    }
}
