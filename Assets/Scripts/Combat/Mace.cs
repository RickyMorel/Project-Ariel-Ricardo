using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : WeaponShoot
{
    #region Editor Fields

    [SerializeField] private GameObject _maceHead;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private float _extensionSpeed = 0.2f;
    [SerializeField] private Vector2 _minAndMaxPositions;

    #endregion

    #region Private Variable

    private Rigidbody _rb;
    private AttackHitBox _attackHitBox;

    #endregion

    private void Awake()
    {
        _rb = _maceHead.GetComponent<Rigidbody>();
        _attackHitBox = _maceHead.GetComponent<AttackHitBox>();

        _attackHitBox.OnHit += HandleHitParticles;
    }

    private void OnDestroy()
    {
        _attackHitBox.OnHit -= HandleHitParticles;
    }

    private void OnEnable()
    {
        _maceHead.transform.parent = null;
    }

    private void OnDisable()
    {
        if(_parentTransform == null) { return; }

        _maceHead.transform.parent = _parentTransform;
    }

    private void FixedUpdate()
    {
        if (_weapon.CurrentPlayer == null) { return; }

        _rb.AddForce(_weapon.CurrentPlayer.MoveDirection * 50);

        //if(Vector3.Distance(transform.position, _maceHead.transform.position) < )
    }

    public override void CheckShootInput()
    {

    }

    public override void Shoot()
    {
        //do nothing
    }

    private void HandleHitParticles(GameObject obj)
    {
        Instantiate(GameAssetsManager.Instance.MeleeFloorHitParticles, _maceHead.transform.position, Quaternion.identity);
        ShipCamera.Instance.ShakeCamera(5f, 50f, 0.2f);
    }
}