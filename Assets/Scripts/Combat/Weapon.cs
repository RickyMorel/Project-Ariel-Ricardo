using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Upgradable
{
    #region Editor Fields

    [Header("Shooting Variables")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _timeBetweenShots = 0.2f;
    [SerializeField] private Transform _shootTransform;

    [Header("Rotation Variables")]
    [SerializeField] private Transform _turretHead;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private Vector2 _rotationLimits; 

    #endregion

    #region Private Variables

    private float _timeSinceLastShot;
    private float _rotationX;

    private WeaponShoot _weaponShoot;

    #endregion

    #region Public Properties

    public GameObject ProjectilePrefab => _projectilePrefab;
    public float TimeBetweenShots => _timeBetweenShots;
    public Transform ShootTransform => _shootTransform;
    public Transform TurretHead => _turretHead;

    #endregion

    #region Getters and Setters

    public float TimeSinceLastShot { get { return _timeSinceLastShot; } set { _timeSinceLastShot = value; } }

    #endregion

    #region Unity Loops

    public override void Awake()
    {
        base.Awake();

        OnUpgradeMesh += HandleUpgrade;
    }

    public override void Start()
    {
        base.Start();

        _rotationX = (_rotationLimits.x + _rotationLimits.y)/2;

        _weaponShoot = GetComponentInChildren<WeaponShoot>();
    }

    private void Update()
    {
        UpdateTime();

        if (_currentPlayer == null) { return; }

        if (CanUse == false) { return; }

        _weaponShoot.CheckShootInput();
        CheckRotationInput();
    }

    private void OnDestroy()
    {
        OnUpgradeMesh -= HandleUpgrade;
    }

    private void UpdateTime()
    {
        _timeSinceLastShot += Time.deltaTime;
    }

    #endregion

    private void HandleUpgrade(GameObject meshObject, GameObject projectileObject)
    {
        Transform rotationChild = meshObject.transform.GetChild(0);

        _turretHead = rotationChild;

        _shootTransform.transform.SetParent(_turretHead);

        _projectilePrefab = projectileObject;
    }

    private void CheckRotationInput()
    {
         if(_currentPlayer.MoveDirection.x == 0) { return; }

        _rotationX += _rotationSpeed * _currentPlayer.MoveDirection.x * Time.deltaTime;
        _rotationX = Mathf.Clamp(_rotationX, _rotationLimits.x, _rotationLimits.y);
        _turretHead.localEulerAngles = new Vector3(_rotationX, 0f, 0f);
    }
}