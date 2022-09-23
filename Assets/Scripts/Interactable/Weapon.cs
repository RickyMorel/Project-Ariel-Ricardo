using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Upgradable
{
    #region Editor Fields

    [SerializeField] protected GameObject _projectilePrefab;
    [SerializeField] protected float _timeBetweenShots = 0.2f;
    [SerializeField] private Transform _shootTransform;

    #endregion

    #region Private Variables

    private float _timeSinceLastShot;

    #endregion

    #region Unity Loops

    private void Update()
    {
        UpdateTime();

        if (_currentPlayer == null) { return; }

        if (_currentPlayer.IsShooting)
        {
            Shoot();
        }
    }

    private void UpdateTime()
    {
        _timeSinceLastShot += Time.deltaTime;
    }

    #endregion

    private void Shoot()
    {
        if(_timeBetweenShots > _timeSinceLastShot) { return; }

        _timeSinceLastShot = 0f;

        Instantiate(_projectilePrefab, _shootTransform.position, transform.rotation);
    }
}
