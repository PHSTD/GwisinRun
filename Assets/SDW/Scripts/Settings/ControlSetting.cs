using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlSetting : MonoBehaviour
{
    [SerializeField] private InputActionAsset m_inputActionAsset;
    
    void Start()
    {
        GameManager.Instance.Input.SetPlayerInputActionAsset(m_inputActionAsset);
    }


    void Update()
    {
        
    }
}
