using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    #region Private Variable

    protected Weapon _weapon;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _weapon = GetComponentInParent<Weapon>();
    }

    #endregion

    public virtual void CheckShootInput()
    {
        if (_weapon.CurrentPlayer.IsUsing)
        {
            Shoot();
        }
    }

    public virtual void Shoot()
    {
        if (_weapon.TimeBetweenShots > _weapon.TimeSinceLastShot) { return; }

        _weapon.TimeSinceLastShot = 0f;

        Instantiate(_weapon.ProjectilePrefab, _weapon.ShootTransform.position, _weapon.TurretHead.rotation);
    }
}