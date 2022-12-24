using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    #region Private Variable

    protected Weapon _weapon;
    protected float _timeBetweenShots = 0.2f;
    protected float _timeSinceLastShot;

    #endregion

    #region Unity Loops

    public virtual void Start()
    {
        _weapon = GetComponentInParent<Weapon>();
    }

    private void Update()
    {
        UpdateTime();
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
        if (_timeBetweenShots > _timeSinceLastShot) { return; }

        _timeSinceLastShot = 0f;

        Instantiate(_weapon.ProjectilePrefab, _weapon.ShootTransform.position, _weapon.TurretHead.rotation);
    }

    private void UpdateTime()
    {
        _timeSinceLastShot += Time.deltaTime;
    }
}