using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHS_Controller : MonoBehaviour
{
    public float moveSpeed = 6;
    private Rigidbody m_rb;
    private Camera m_viewCamera;
    private Vector3 m_velocity;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_viewCamera = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = m_viewCamera.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, //마우스 스크린 좌표
            m_viewCamera.transform.position.y)); //카메라 위치 y좌표
            transform.LookAt(mousePos + Vector3.up * transform.position.y);
            //transform.forward 방향을 해당 위치를 향하도록 회전

        m_velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;

    }
    void FixedUpdate()
    {
        m_rb.MovePosition(m_rb.position + m_velocity * Time.fixedDeltaTime);
    }
}
