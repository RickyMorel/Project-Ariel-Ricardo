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
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private Vector2 _rotationLimits; 

    #endregion

    #region Private Variables

    private float _timeSinceLastShot;
    private float _rotationX;

    #endregion

    #region Unity Loops

    public override void Start()
    {
        base.Start();

        _rotationX = (_rotationLimits.x + _rotationLimits.y)/2;
    }

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
         if(_currentPlayer.MoveDirection.x == 0) { return; }

        _rotationX += _rotationSpeed * _currentPlayer.MoveDirection.x * Time.deltaTime;
        _rotationX = Mathf.Clamp(_rotationX, _rotationLimits.x, _rotationLimits.y);
        _turretHead.localEulerAngles = new Vector3(_rotationX, 0f, 0f);
    }

    private void CheckShootInput()
    {
        if (_currentPlayer.IsUsing)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if(_timeBetweenShots > _timeSinceLastShot) { return; }

        _timeSinceLastShot = 0f;

        Instantiate(_projectilePrefab, _shootTransform.position, _turretHead.rotation);
    }
}
