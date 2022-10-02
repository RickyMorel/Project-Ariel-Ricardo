using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : RotationalInteractable
{

    #region Editor Fields

    [SerializeField] private float _acceleration = 1.0f;
    [SerializeField] private float _topSpeed = 200f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ParticleSystem _boosterParticle;

    #endregion

    #region Private Variables

    [SerializeField] private bool _isBoosting = false;

    #endregion

    #region Public Properties

    public static event Action<bool> OnBoostUpdated;

    #endregion

    #region Unity Loops

    public override void Update()
    {
        base.Update();
       
        if (_currentPlayer == null) { return; }
        
        if (!_currentPlayer.IsShooting) { SetIsBoosting(false); return; }

        SetIsBoosting(true);
    }

    private void FixedUpdate()
    {
        BoostShip();

        _boosterParticle.Stop();
    }

    #endregion

    private void SetIsBoosting(bool isBoosting)
    {
        //If value is the same, don't update
        if(_isBoosting == isBoosting) { return; }

        _isBoosting = isBoosting;

        OnBoostUpdated?.Invoke(isBoosting);
    }

    private void BoostShip()
    {
        if (!_isBoosting) { return; }

        _boosterParticle.Play();

        _rb.AddForce(-(RotatorTransform.transform.up * _acceleration * _rb.mass));

        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _topSpeed);
    }
}