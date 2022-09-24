using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Upgradable
{
    #region Editor Fields

    [Header("Shooting Variables")]
    [SerializeField] protected GameObject _projectilePrefab;
    [SerializeField] protected float _timeBetweenShots = 0.2f;
    [SerializeField] private Transform _shootTransform;

    [Header("Rotation Variables")]
    [SerializeField] private Transform _turretHead;
    [SerializeField] private Vector2 _rotationLimits; 

    #endregion

    #region Private Variables

    private float _timeSinceLastShot;

    #endregion

    #region Unity Loops

    private void Update()
    {
        UpdateTime();

        if (_currentPlayer == null) { return; }

        CheckShootInput();
        CheckRotationInput();
    }

    private void UpdateTime()
    {
        _timeSinceLastShot += Time.deltaTime;
    }

    #endregion

    private void CheckRotationInput()
    {
        //float rotationAngle = 
        //_turretHead.Rotate()
    }

    private void CheckShootInput()
    {
        if (_currentPlayer.IsShooting)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if(_timeBetweenShots > _timeSinceLastShot) { return; }

        _timeSinceLastShot = 0f;

        Instantiate(_projectilePrefab, _shootTransform.position, transform.rotation);
    }
}
