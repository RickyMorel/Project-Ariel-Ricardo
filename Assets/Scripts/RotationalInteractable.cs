using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationalInteractable : Upgradable
{
    #region Editor Fields

    [SerializeField] private Transform _rotatorTransform;
    [SerializeField] private Transform _pivotTransform;

    #endregion

    #region Private variables

    private float _currentAngle, _shieldRotationSpeed;

    #endregion

    #region Public variables



    #endregion

private void RotateShield()
    {
        _currentAngle = _shieldRotationSpeed * _currentPlayer.MoveDirection.x;
        _rotatorTransform.RotateAround(_pivotTransform.position, Vector3.forward, _currentAngle);
    }
}
