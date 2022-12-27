using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShotgun : WeaponShoot
{
    #region Private Variable

    private bool _hasAlreadyShot = false;

    #endregion

    public override void Start()
    {
        _weapon = GetComponentInParent<Weapon>();
        _timeBetweenShots = 0.7f;
        _amountOfProjectiles = 5;
    }

    public override void CheckShootInput()
    {
        if (_weapon.CurrentPlayer.IsUsing && !_hasAlreadyShot)
        {
            Shoot();
            _hasAlreadyShot = true;
        }
        else if (!_weapon.CurrentPlayer.IsUsing && _hasAlreadyShot)
        {
            _hasAlreadyShot = false;
        }
    }

    public override void Shoot()
    {
        if (_timeBetweenShots > _timeSinceLastShot) { return; }

        _timeSinceLastShot = 0f;

        ProjectileShootAngle();
    }
}