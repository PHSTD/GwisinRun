using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool RunKeyPressed { get; private set; }
    public bool RunKeyBeingHeld { get; private set; }
    public bool RunKeyReleased { get; private set; }
    public bool SitKeyPressed { get; private set; }
    public bool SitKeyBeingHeld { get; private set; }
    public bool SitKeyReleased { get; private set; }
    public bool InteractionKeyPressed { get; private set; }

    private PlayerInput m_playerInput;
    private InputAction m_moveAction;
    private InputAction m_runAction;
    private InputAction m_sitAction;
    private InputAction m_interactionAction;

    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void SetupInputActions()
    {
        m_moveAction = m_playerInput.actions["Move"];
        m_runAction = m_playerInput.actions["Run"];
        m_sitAction = m_playerInput.actions["Sit"];
        m_interactionAction = m_playerInput.actions["Interaction"];
    }

    private void UpdateInputs()
    {
        MoveInput = m_moveAction.ReadValue<Vector2>();
        RunKeyPressed = m_runAction.WasPressedThisFrame();
        RunKeyBeingHeld = m_runAction.IsPressed();
        RunKeyReleased = m_runAction.WasReleasedThisFrame();
        SitKeyPressed = m_sitAction.WasPressedThisFrame();
        SitKeyBeingHeld = m_sitAction.IsPressed();
        SitKeyReleased = m_sitAction.WasReleasedThisFrame();
        InteractionKeyPressed = m_interactionAction.WasPressedThisFrame();
    }
}
