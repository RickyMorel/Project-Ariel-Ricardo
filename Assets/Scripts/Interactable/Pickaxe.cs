using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : RotationalInteractable
{
    #region Editor Fields

    [SerializeField] private float _acceleration = 1.0f;
    [SerializeField] private float _topAcleration = 21f;
    [SerializeField] private float _pickaxeDrag = 0.1f;
    [SerializeField] private float _noramlTopSpeed = 200f;
    [SerializeField] private float _boostTopSpeed = 200f;

    #endregion

    #region Private Variables

    private float _initialRotationSpeed;
    private float _topSpeed;
    private bool _isBoosting = false;

    #endregion

    #region Unity Loops

    public override void Start()
    {
        base.Start();

        _initialRotationSpeed = RotationSpeed;
    }

    public override void Update()
    {
        base.Update();

        if (_currentPlayer == null) { SetIsBoosting(false); return; }

        if (!_currentPlayer.IsUsing) { SetIsBoosting(false); return; }

        SetIsBoosting(true);
    }

    private void FixedUpdate()
    {
        BoostPickaxe();
        ApplyDrag();

    }   

    #endregion

    private void SetIsBoosting(bool isBoosting)
    {
        //If value is the same, don't update
        if (_isBoosting == isBoosting) { return; }

        _isBoosting = isBoosting;
    }

    private void ApplyDrag()
    {
        if(CurrentAngle == 0) { return; }

        if((_isBoosting || _currentPlayer.MoveDirection.magnitude > 0) && CurrentAngle < _topSpeed) { return; }

        CurrentAngle -= CurrentAngle * _pickaxeDrag * Time.deltaTime;
    }

    private void BoostPickaxe()
    {
        _topSpeed = _isBoosting ? _boostTopSpeed : _noramlTopSpeed;
    }

    public override void Rotate()
    {
        float acceleration = RotationSpeed * _currentPlayer.MoveDirection.x * Time.deltaTime;

        //Start slowing down if surpassed top speed
        if (CurrentAngle > _topSpeed) { acceleration = 0; }

        CurrentAngle = Mathf.Clamp(CurrentAngle + acceleration, -_initialRotationSpeed, _boostTopSpeed);

        RotatorTransform.RotateAround(PivotTransform.position, Vector3.forward, -CurrentAngle);
    }
}
