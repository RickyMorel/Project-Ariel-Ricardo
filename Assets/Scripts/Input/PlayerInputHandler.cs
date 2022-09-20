using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Private Variables

    private Vector2 _moveDirection;

    #endregion

    #region Public Properties

    public event Action OnJump;
    public Vector2 MoveDirection => _moveDirection;

    #endregion

    public void Move(InputAction.CallbackContext obj)
    {
        _moveDirection = obj.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }
}
