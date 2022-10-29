using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerCarryController))]
public class PlayerStateMachine : BaseStateMachine
{
    #region Getters & Setters


    #endregion

    #region Private Variables

    private PlayerInputHandler _playerInput;
    private float _turnSmoothVelocity;
    private bool _isAttachedToShip;

    #endregion

    #region Public Properties

    public bool IsAttachedToShip => _isAttachedToShip;
    public override bool IsShooting => _playerInput == null ? false : _playerInput.IsShooting;

    #endregion

    #region Unity Loops

    public override void Awake()
    {
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    public override void Start()
    {
        _playerInput = GetComponent<PlayerInputHandler>();
        _playerInteraction = GetComponent<PlayerInteractionController>();
        _playerRagdoll = GetComponent<PlayerRagdoll>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerCarryController = GetComponent<PlayerCarryController>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        AttachToShip(true);

        _playerInput.OnJump += HandleJump;
    }

    public void AttachToShip(bool isAttached)
    {
        _isAttachedToShip = isAttached;

        if (isAttached)
        {
            transform.SetParent(Ship.Instance.transform);
        }
        else
        {
            transform.parent = null;
        }
    }

    public override void Update()
    {
        _currentState.UpdateStates();
    }

    public override void FixedUpdate()
    {
        if (_playerInteraction.IsInteracting()) { return; }

        if (_canMove)
        {
            Move();
            RotateTowardsMove();
            AnimateMove();
        }

        CheckIfFellOutOfShip();
    }

    public void OnDestroy()
    {
        if (_playerInput == null) { return; }

        _playerInput.OnJump -= HandleJump;
    }

    #endregion

    public override void Move()
    {
        float cappedSpeed = _currentSpeed / 20;
        _moveDirection = new Vector3(_playerInput.MoveDirection.x * cappedSpeed, 0f, _playerInput.MoveDirection.y * cappedSpeed);
        transform.position += _moveDirection;
    }

    public override void RotateTowardsMove()
    {
        float targetAngle = Mathf.Atan2(_playerInput.MoveDirection.x, _playerInput.MoveDirection.y) * Mathf.Rad2Deg;

        if (_playerInput.MoveDirection.magnitude == 0)
            targetAngle = 180f;

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public override void AnimateMove()
    {
        _anim.SetFloat("Movement", _playerInput.MoveDirection.magnitude);
    }

    private void CheckIfFellOutOfShip()
    {
        if (!_isAttachedToShip) { return; }

        if(Vector3.Distance(transform.position, Ship.Instance.transform.position) < 50) { return; }

        transform.localPosition = Vector3.zero;
    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
    }
}
