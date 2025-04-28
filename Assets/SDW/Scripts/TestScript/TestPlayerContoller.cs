using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerContoller : MonoBehaviour
{
    private Vector2 m_moveInput;
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private float m_moveSpeed;

    private void Start()
    {
    }

    private void Update()
    {
        Move();
        ActionKeys();
    }

    private void Move()
    {
        m_moveInput = GameManager.Instance.Input.MoveInput;
        m_rigidbody.velocity = new Vector3(m_moveInput.x * m_moveSpeed, 0, m_moveInput.y * m_moveSpeed);
    }

    private void ActionKeys()
    {
        //# 뛰기
        if (GameManager.Instance.Input.RunKeyPressed)
        {
            Debug.Log("Run Key Pressed");
        }
        if (GameManager.Instance.Input.RunKeyBeingHeld)
        {
            Debug.Log("Run Key Being Held");
        }
        if (GameManager.Instance.Input.RunKeyReleased)
        {
            Debug.Log("Run Key Released");
        }
        
        //# 앉기
        if (GameManager.Instance.Input.SitKeyPressed)
        {
            Debug.Log("Sit Key Pressed");
        }
        if (GameManager.Instance.Input.SitKeyBeingHeld)
        {
            Debug.Log("Sit Key Being Held");
        }
        if (GameManager.Instance.Input.SitKeyReleased)
        {
            Debug.Log("Sit Key Released");
        }
        
        //# 상호작용
        if (GameManager.Instance.Input.InteractionKeyPressed)
        {
            Debug.Log("상호작용 키");
        }
    }
}
