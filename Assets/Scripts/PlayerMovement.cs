using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    #region Editor Fields

    [Header("Movement")]
    [SerializeField] private float _speed;

    [Header("Rotation")]
    [SerializeField] private float _turnSmoothTime;

    [Header("Jumping")]
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _gravityIntensity = -15f;
    [SerializeField] private float _jumpCoolDown = 2f;
    [SerializeField] private bool _canJump;
    [SerializeField] private bool _isGrounded;

    #endregion

    #region Private Variables

    private PlayerInput _playerInput;
    private Animator _anim;
    private Rigidbody _rb;

    private Vector3 _moveDirection;
    private float _turnSmoothVelocity;
    private float _timeSinceLastJump;

    #endregion

    #region Public Properties

    #endregion

    #region Unity Loops

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        _playerInput.OnJump += HandleJump;
    }

    private void OnDestroy()
    {
        _playerInput.OnJump -= HandleJump;
    }

    private void Update()
    {
        UpdateTime();
    }

    private void FixedUpdate()
    {
        Move();
        RotateTowardsMove();
        UpdateJumpState();
        AnimateMove();
    }

    private void UpdateTime()
    {
        _timeSinceLastJump += Time.deltaTime;
    }

    #endregion

    #region Movement && Rotation

    private void Move()
    {
        float cappedSpeed = _speed / 20;
        _moveDirection = new Vector3(_playerInput.MoveDirection.x * cappedSpeed, 0f, _playerInput.MoveDirection.y * cappedSpeed);
        transform.position += _moveDirection;
    }

    private void RotateTowardsMove()
    {
        float targetAngle = Mathf.Atan2(_playerInput.MoveDirection.x, _playerInput.MoveDirection.y) * Mathf.Rad2Deg;

        if (_playerInput.MoveDirection.magnitude == 0)
            targetAngle = 180f;

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void AnimateMove()
    {
        _anim.SetFloat("Movement", _playerInput.MoveDirection.magnitude);
    }

    #endregion

    #region Jumping && Falling
    public void SetIsGrounded(bool isGrounded)
    {
        _isGrounded = isGrounded;
    }

    private void UpdateJumpState()
    {
        if(_timeSinceLastJump > _jumpCoolDown) { _canJump = true; }

        _anim.SetBool("isFalling", !_isGrounded);

        //Don't play jump anim while in air 
        if (!_isGrounded) { _anim.ResetTrigger("Jump"); }
    }

    private void HandleJump()
    {
        if (!_canJump || !_isGrounded) { return; }

        _anim.SetTrigger("Jump");

        _canJump = false;

        _timeSinceLastJump = 0f;

        float jumpingVelocity = Mathf.Sqrt(-2 * _gravityIntensity * _jumpHeight);
        Vector3 playerVelocity = _moveDirection;
        playerVelocity.y = jumpingVelocity;
        _rb.velocity = playerVelocity;
    }

    #endregion
}
