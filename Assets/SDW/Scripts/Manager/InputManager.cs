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
    public bool DropKeyPressed { get; private set; }
    public bool[] ItemKeyPressed { get; private set; }
    public bool PauseKeyPressed { get; private set; }
    public bool JumpKeyPressed { get; private set; }

    private PlayerInput m_playerInput;
    
    private InputAction m_moveAction;
    private InputAction m_runAction;
    private InputAction m_sitAction;
    private InputAction m_interactionAction;
    private InputAction m_dropAction;
    private InputAction m_itemsAction;
    private InputAction m_pauseAction;
    private InputAction m_jumpAction;
    private bool m_isUpdateable;

    private void Awake()
    {
        ItemKeyPressed = new bool[6];
    }

    private void Update()
    {
        if(m_isUpdateable == true)
            UpdateInputs();
    }

    private void SetupInputActions()
    {
        m_moveAction = m_playerInput.actions["Move"];
        m_runAction = m_playerInput.actions["Run"];
        m_sitAction = m_playerInput.actions["Sit"];
        m_interactionAction = m_playerInput.actions["Interaction"];
        m_dropAction = m_playerInput.actions["Drop"];
        m_itemsAction = m_playerInput.actions["Items"];
        m_pauseAction = m_playerInput.actions["Pause"];
        m_jumpAction = m_playerInput.actions["Jump"];

        //# Pressed
        m_itemsAction.started += OnInteractionStarted;
        //# BeingHeld
        // m_itemsAction.performed
        //# Released
        // m_itemsAction.canceled
    }

    public void SetPlayerInput(PlayerInput playerInput)
    {
        m_playerInput = playerInput;
        SetupInputActions();
        m_isUpdateable = true;
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
        DropKeyPressed = m_dropAction.WasPressedThisFrame();
        PauseKeyPressed = m_pauseAction.WasPressedThisFrame();
        JumpKeyPressed= m_jumpAction.WasPressedThisFrame();
    }

    private void OnInteractionStarted(InputAction.CallbackContext context)
    {
        var control = context.control;

        int index = int.Parse(control.name) - 1;
        ItemKeyPressed[index] = true;
    }
}
