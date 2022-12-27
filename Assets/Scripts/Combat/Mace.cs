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

    private bool _isCurrentlyShooting = false;
    private Rigidbody _rb;

    #endregion

    private void Awake()
    {
        _rb = _maceHead.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _maceHead.transform.parent = null;
    }

    private void OnDisable()
    {
        _maceHead.transform.parent = _parentTransform;
    }

    private void FixedUpdate()
    {
        if (_weapon.CurrentPlayer == null) { return; }

        _rb.AddForce(_weapon.CurrentPlayer.MoveDirection * 50);

        //float wantedPos = _minAndMaxPositions.x;

        //if (_isCurrentlyShooting) { wantedPos = _minAndMaxPositions.y; }

        //if (_maceHead.transform.localPosition.y == wantedPos) { return; }

        //float moveTo = _maceHead.transform.localPosition.y + _extensionSpeed * Time.deltaTime;
        //float yMovePos = Mathf.Clamp(moveTo, _minAndMaxPositions.x, _minAndMaxPositions.y);
        //_maceHead.transform.localPosition = new Vector3(_maceHead.transform.localPosition.x, wantedPos, _maceHead.transform.localPosition.z);
    }

    public override void CheckShootInput()
    {
        _isCurrentlyShooting = _weapon.CurrentPlayer.IsUsing;
    }

    public override void Shoot()
    {
        //do nothing
    }
}