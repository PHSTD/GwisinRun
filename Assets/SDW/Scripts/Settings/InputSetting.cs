using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSetting : MonoBehaviour
{
    void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        GameManager.Instance.Input.SetPlayerInput(playerInput);
    }
}
