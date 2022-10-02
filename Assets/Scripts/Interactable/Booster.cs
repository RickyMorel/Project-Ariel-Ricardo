using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : RotationalInteractable
{

    #region Editor Fields

    [SerializeField] private float _acceleration = 1.0f;
    [SerializeField] private float _boostImpulseForce = 50f;
    [SerializeField] private float _topSpeed = 200f;
    [SerializeField] private List<Gear> _gears = new List<Gear>();
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ParticleSystem _boosterParticle;

    #endregion

    #region Private Variables

    private bool _isBoosting = false;
    private int _currentGear = 0;

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
        CheckGears();

        _boosterParticle.Stop();
    }

    #endregion

    private void SetIsBoosting(bool isBoosting)
    {
        //If value is the same, don't update
        if(_isBoosting == isBoosting) { return; }

        _isBoosting = isBoosting;

        OnBoostUpdated?.Invoke(isBoosting);

        if (isBoosting)
            BoostImpulse();
    }

    private void BoostImpulse()
    {
        _rb.AddForce(-(RotatorTransform.transform.up * _boostImpulseForce * _rb.mass), ForceMode.Impulse);
    }

    private void BoostShip()
    {
        if (!_isBoosting) { return; }

        _boosterParticle.Play();

        _rb.AddForce(-(RotatorTransform.transform.up * _acceleration * _rb.mass));

        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _topSpeed);
    }

    private void CheckGears()
    {
        if(_rb.velocity.magnitude > _gears[_currentGear].MaxSpeed)
        {
            _currentGear = Mathf.Clamp(_currentGear+1, 0, _gears.Count-1);
            Debug.Log("HIGHER GEAR: " + _currentGear);
        }
        else if(_currentGear != 0 && _rb.velocity.magnitude < _gears[_currentGear-1].MaxSpeed)
        {
            _currentGear = Mathf.Clamp(_currentGear - 1, 0, _gears.Count - 1);
            Debug.Log("LOWER GEAR: " + _currentGear);
        }
    }
}

#region Helper Classes

[System.Serializable]
public class Gear
{
    public float MaxSpeed;
}

#endregion