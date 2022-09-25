using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : RotationalInteractable
{

    #region Editors Fields

    [SerializeField] private float _boosterSpeed = 1.0f;

    #endregion

    #region Private Variables

    private Rigidbody _rb;

    #endregion

    #region Unity Loops


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void Update()
    {
        base.Update();
        if (_currentPlayer == null) { return; }
        if (!_currentPlayer.IsShooting) { return; }
        BoostingTheShip();
    }
    #endregion

    private void BoostingTheShip()
    {
        _rb.AddForce(-(RotatorTransform.transform.up * _boosterSpeed));
    }
}