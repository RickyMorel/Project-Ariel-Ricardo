using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Transform _rotationCenter;

    #endregion

    #region Private Variables

    private float _posX, _posY, _angle = 0;
    private PlayerInputHandler _playerInput;
    #endregion
    
    #region Public Properties

    public float AngularSpeed, RotationRadius;
    public Transform RotationCenter;

    #endregion
    
    #region Unity Loops
    
    private void Start() 
    {
        _playerInput = FindObjectOfType<PlayerInputHandler>();
    }

    private void Update() 
    {
        Rotate();
    }
    #endregion

    private void Rotate()
    {
        _angle = (Mathf.Acos(_playerInput.MoveDirection.x) + Mathf.Asin(_playerInput.MoveDirection.y))/2;
        _rotationCenter.transform.rotation = Quaternion.Euler(0, 0, _angle);
    }
}