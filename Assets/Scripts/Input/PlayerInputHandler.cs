using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Private Variables

    private Vector2 _moveDirection;
    private bool _isShooting;

    #endregion

    #region Public Properties

    public event Action<InputAction.CallbackContext> OnJump;
    public static event Action<PlayerInputHandler, bool> OnSpecialAction;

    public event Action OnInteract;
    
    public event Action OnUpgrade;
   
    public Vector2 MoveDirection => _moveDirection;
   
    public bool IsShooting => _isShooting;

    #endregion

    public void Move(InputAction.CallbackContext obj)
    {
        _moveDirection = obj.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke(obj);
    }

    public void SpecialAction(InputAction.CallbackContext obj)
    {
        //prevents from spam calling this function
        if (!obj.started) { return; }

        bool value = obj.ReadValue<float>() == 1f ? true : false;
        OnSpecialAction?.Invoke(this, value);
    }

    public void Interact(InputAction.CallbackContext obj)
    {
        //prevents from spam calling this function
        if (!obj.started) { return; }

        OnInteract?.Invoke();
    }

    public void Upgrade(InputAction.CallbackContext obj)
    {
        //prevents from spam calling this function
        if (!obj.started) { return; }

        OnUpgrade?.Invoke();
    }

    public void Shoot(InputAction.CallbackContext obj)
    {
        _isShooting = obj.ReadValue<float>() == 1f ? true : false;
    }
}
