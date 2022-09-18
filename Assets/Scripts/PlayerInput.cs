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

    private Vector2 _moveDirection;

    #endregion

    #region Public Properties

    public Vector2 MoveDirection => _moveDirection;

    #endregion

    private void Awake()
    {
        _inputMaster = new InputMaster();
    }

    private void OnEnable()
    {
        _move = _inputMaster.Player.Move;

        _move.Enable();
    }

    private void OnDisable()
    {
        _move.Disable();
    }

    private void Update()
    {
        _moveDirection = _move.ReadValue<Vector2>();
    }
}
