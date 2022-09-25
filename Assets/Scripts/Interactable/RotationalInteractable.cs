using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationalInteractable : Upgradable
{
    #region Editor Fields

    [SerializeField] public Transform RotatorTransform;
    
    [SerializeField] private Transform _pivotTransform;

    #endregion

    #region Private variables

    private float _currentAngle = 0, _shieldRotationSpeed = 1;

    #endregion

    #region Public variables



    #endregion

    #region Unity Loops

    public virtual void Update()
    {
        if (_currentPlayer == null) { return; }
        
        Rotate();
    }

    #endregion

    private void Rotate()
    {
        if (_currentPlayer.MoveDirection.magnitude == 0) { return; }
        _currentAngle = _shieldRotationSpeed * _currentPlayer.MoveDirection.x;
        RotatorTransform.RotateAround(_pivotTransform.position, Vector3.forward, -_currentAngle);
    }
}
