using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMinigun : WeaponShoot
{
    #region Private Variables

    private int _shootNumber = -1;

    #endregion

    #region Unity Loops

    public override void Update()
    {
        base.Update();

        if (_isShooting) { WeaponBarrelRoll(_maxRotationSpeed); }
    }

    #endregion

    public override void CheckShootInput()
    {
        if (_weapon.CurrentPlayer.IsUsing)
        {
            Shoot();
        }
        else
        {
            _isShooting = false;
            if (_rotationSpeed <= 0) { return; }
            _rotationSpeed = _rotationSpeed - Time.deltaTime * 200;
            WeaponBarrelRoll(_rotationSpeed);
        }
    }

    public override void Shoot()
    {
        if (_timeBetweenShots > _timeSinceLastShot) { return; }

        if (_rotationSpeed < _maxRotationSpeed)
        {
            _rotationSpeed = _rotationSpeed + Time.deltaTime * 200;
            WeaponBarrelRoll(_rotationSpeed);
            return;
        }

        _isShooting = true;

        if (_shootNumber < _weapon.ShootTransform.Length - 1) 
        { 
            _shootNumber++; 
            ProjectileShootFromOtherBarrels(_shootNumber);
            _timeSinceLastShot = 0f;
        }

        else { _shootNumber = -1; }
    }
}