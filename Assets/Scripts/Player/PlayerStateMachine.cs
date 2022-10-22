using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    #region Editor Fields

    [Header("Movement")]
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _runSpeed;

    [Header("Rotation")]
    [SerializeField] private float _turnSmoothTime;

    [Header("Jumping")]
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _gravityIntensity = -15f;
    [SerializeField] private bool _isGrounded;

    [Header("State Variables")]
    [SerializeField] private PlayerBaseState _currentState;
    [SerializeField] private PlayerStateFactory _states;

    #endregion

    #region Getters & Setters

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public float Speed { get { return _currentSpeed; } set { _currentSpeed = value; } }
    public float JumpHeight { get { return _jumpHeight; } set { _jumpHeight = value; } }
    public float GravityIntensity { get { return _gravityIntensity; } set { _gravityIntensity = value; } }
    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }

    #endregion

    #region Private Variables

    private PlayerInputHandler _playerInput;
    private PlayerInteractionController _playerInteraction;
    private PlayerRagdoll _playerRagdoll;
    private PlayerHealth _playerHealth;
    private PlayerCarryController _playerCarryController;
    private Animator _anim;
    private Rigidbody _rb;

    private float _turnSmoothVelocity;

    private Vector3 _moveDirection;
    private bool _isJumpPressed;
    private bool _canMove = true;
    private bool _isAttachedToShip;

    #endregion

    #region Public Properties

    public PlayerInteractionController PlayerInteraction => _playerInteraction;
    public PlayerRagdoll PlayerRagdoll => _playerRagdoll;
    public PlayerHealth PlayerHealth => _playerHealth;
    public PlayerCarryController PlayerCarryController => _playerCarryController;
    public Animator Anim => _anim;
    public Rigidbody Rb => _rb;
    public Vector3 MoveDirection => _moveDirection;
    public bool IsAttachedToShip => _isAttachedToShip;
    public float RunSpeed => _runSpeed;
    public bool IsJumpPressed => _isJumpPressed;
    public bool IsShooting => _playerInput == null ? false : _playerInput.IsShooting;

    #endregion

    #region Unity Loops

    private void Awake()
    {
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    private void Start()
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

    private void AttachToShip(bool isAttached)
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

    private void Update()
    {
        _currentState.UpdateStates();
    }

    private void FixedUpdate()
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

    private void OnDestroy()
    {
        if (_playerInput == null) { return; }

        _playerInput.OnJump -= HandleJump;
    }

    #endregion

    private void Move()
    {
        float cappedSpeed = _currentSpeed / 20;
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
