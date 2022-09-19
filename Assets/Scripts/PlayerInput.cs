using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private InputMaster _inputMaster;

    #endregion

    #region Private Variables

    private InputAction _move;
    private InputAction _jump;

    private Vector2 _moveDirection;

    #endregion

    #region Public Properties

    public event Action OnJump;
    public Vector2 MoveDirection => _moveDirection;

    #endregion

    private void Awake()
    {
        _inputMaster = new InputMaster();
    }

    private void OnEnable()
    {
        _move = _inputMaster.Player.Move;
        _jump = _inputMaster.Player.Jump;

        _jump.performed += Jump;

        _move.Enable();
        _jump.Enable();
    }

    private void OnDisable()
    {
        _jump.performed -= Jump;

        _move.Disable();
        _jump.Disable();
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }

    private void Update()
    {
        _moveDirection = _move.ReadValue<Vector2>();
    }
}
