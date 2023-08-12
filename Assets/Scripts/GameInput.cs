using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteract;
    public event EventHandler OnInteractAlt;
    public event EventHandler OnPause;
    private PlayerInputAction inputAction;
    public static GameInput Instance { get; private set; }
    public void Awake()
    {
        Instance = this;
        inputAction = new PlayerInputAction();
        inputAction.Player.Enable();

        inputAction.Player.Interact.performed += OnInteractPerformed;
        inputAction.Player.InterfactAlt.performed += OnInteractAltPerformed;
        inputAction.Player.Pause.performed += OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteractAltPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAlt?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteractPerformed(InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetMovementVectorNormalized()
    {
        var moveVector = inputAction.Player.Move.ReadValue<Vector2>();

        return new Vector3(moveVector.x, 0, moveVector.y).normalized;
    }

    public bool IsSprintKeyPressed()
    {
        return inputAction.Player.Sprint.IsPressed();
    }

    public bool IsInteractKeyPressed()
    {
        return inputAction.Player.Interact.IsPressed();
    }
}