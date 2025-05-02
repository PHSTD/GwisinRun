using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCamera : MonoBehaviour
{
    [Header("Player Sight Settings")]
    [SerializeField] private float m_zoomFOV = 30f;
    [SerializeField] private float m_normalFOV = 60f;
    [SerializeField] private float m_zoomSpeed = 10f;
    
    [Header("Camera Settings")]
    [SerializeField] private Transform m_cameraTransform;
    public Camera PlayerCamera;
    
    private void Start()
    {
        PlayerCamera = m_cameraTransform.GetComponent<Camera>();
        PlayerCamera.fieldOfView = m_normalFOV;
    }

    private void Update()
    {
        //# 수정 사항(20250502) -- 시작 1
        // if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
        //     return;
        //# 수정 사항(20250502) -- 끝
        
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

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(mouseX * 2f * Vector3.up);

        float rotationX = m_cameraTransform.localEulerAngles.x - mouseY * 2f;
        if (rotationX > 180) rotationX -= 360;
        rotationX = Mathf.Clamp(rotationX, -80, 80);
        m_cameraTransform.localEulerAngles = new Vector3(rotationX, 0, 0);
    }

}
