using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    #region Editor Fields

    [Header("Movement")]
    [SerializeField] private float _speed;

    [Header("Rotation")]
    [SerializeField] private float _turnSmoothTime;

    #endregion

    #region Private Variables

    private PlayerInput _playerInput;
    private Animator _anim;

    private float _turnSmoothVelocity;

    #endregion

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        RotateTowardsMove();
        AnimateMove();
    }

    private void Move()
    {
        float cappedSpeed = _speed / 20;
        transform.position += new Vector3(_playerInput.MoveDirection.x * cappedSpeed, 0f, _playerInput.MoveDirection.y * cappedSpeed);
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
}
