using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private void Start()
    {
        // PlayerCamera = m_cameraTransform.GetComponent<Camera>();
        // 메인 카메라를 찾아 저장한다
        GameObject camObj = GameObject.FindWithTag("MainCamera");
        PlayerCamera = camObj.GetComponent<Camera>();
        m_cameraTransform = PlayerCamera.transform;

        // 초점 처리를 위해
        PlayerCamera.fieldOfView = m_normalFOV;
    }

    private void Update()
    {
        RotateView();
        ZoomView();
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
        if (GameManager.Instance.Input.PauseKeyPressed)
        {
            //# 수정 사항(20250502) -- 시작
            if (PlayerController.GameStartPanel != null && PlayerController.GameStartPanel.activeSelf == true)
                return;
            
            if (GameManager.Instance.IsPaused == false)
            {
                PlayerController.PausedMenu.SetActive(true);
            }
            else
            {
                PlayerController.PausedMenu.SetActive(false);
                PlayerController.PausedMenu.GetComponent<PauseMenuUI>().Close();
            }
            //# 수정 사항(20250502) -- 끝
        }

        if (GameManager.Instance.IsPaused)
        {
            return;
        }
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(mouseX * 2f * Vector3.up);

        float rotationX = m_cameraTransform.localEulerAngles.x - mouseY * 2f;
        if (rotationX > 180) rotationX -= 360;
        rotationX = Mathf.Clamp(rotationX, -80, 80);
        m_cameraTransform.localEulerAngles = new Vector3(rotationX, 0, 0);
    }

}
