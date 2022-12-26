using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    #region Private Variable

    protected Weapon _weapon;
    protected float _timeBetweenShots = 0.2f;
    protected float _timeSinceLastShot;
    protected float _shootAngleCone = 90;
    protected int _amountOfProjectiles = 1;

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

        for (int i = 0; i < _amountOfProjectiles; i += 1)
        {
            ProjectileInstantiate();
        }
    }

    private void ProjectileInstantiate()
    {
        Instantiate(_weapon.ProjectilePrefab, _weapon.ShootTransform.position, _weapon.TurretHead.rotation);
    }

    private void UpdateTime()
    {
        _timeSinceLastShot += Time.deltaTime;
    }
}