using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private Vector2 _moveDirection;
    private bool _isShooting;
    [SerializeField] private bool _canPlayerSpawn = false;

    #endregion

    #region Public Properties

    public bool IsPlayerActive = true;

    public event Action<InputAction.CallbackContext> OnJump;
    public static event Action<PlayerInputHandler, bool> OnSpecialAction;

    public event Action OnInteract;
    public event Action OnConfirm;
    public event Action OnCancel;
    public event Action OnUpgrade;
    public event Action<PlayerInputHandler> OnTrySpawn;
   
    public Vector2 MoveDirection => _moveDirection;
   
    public bool IsShooting => _isShooting;

    #endregion

    #region Getters And Setters

    public bool CanPlayerSpawn { get { return _canPlayerSpawn; } set { _canPlayerSpawn = value; } }

    #endregion

    public void Move(InputAction.CallbackContext obj)
    {
        if (!IsPlayerActive) { return; }

        _moveDirection = obj.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        if (CanPlayerSpawn)
        {
            OnTrySpawn?.Invoke(this);
        }

        else if (IsPlayerActive)
        {
            OnJump?.Invoke(obj);
        }
    }

    public void Confirm(InputAction.CallbackContext obj)
    {
        if (!IsPlayerActive) { return; }

        //prevents from spam calling this function
        if (!obj.started) { return; }

        OnConfirm?.Invoke();
    }

    public void Cancel(InputAction.CallbackContext obj)
    {
        if (!IsPlayerActive) { return; }

        //prevents from spam calling this function
        if (!obj.started) { return; }

        OnCancel?.Invoke();
    }

    public void SpecialAction(InputAction.CallbackContext obj)
    {
        if (!IsPlayerActive) { return; }

        //prevents from spam calling this function
        if (!obj.started) { return; }

        bool value = obj.ReadValue<float>() == 1f ? true : false;
        OnSpecialAction?.Invoke(this, value);
    }

    public void Interact(InputAction.CallbackContext obj)
    {
        if (!IsPlayerActive) { return; }

        //prevents from spam calling this function
        if (!obj.started) { return; }

        OnInteract?.Invoke();
    }

    public void Upgrade(InputAction.CallbackContext obj)
    {
        if (!IsPlayerActive) { return; }

        //prevents from spam calling this function
        if (!obj.started) { return; }

        OnUpgrade?.Invoke();
    }

    public void Shoot(InputAction.CallbackContext obj)
    {
        if (!IsPlayerActive) { return; }

        _isShooting = obj.ReadValue<float>() == 1f ? true : false;
    }
}