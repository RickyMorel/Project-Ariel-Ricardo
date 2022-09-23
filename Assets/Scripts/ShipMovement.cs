using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Transform _shipBooster;

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
        _posX = RotationCenter.position.x + _playerInput.MoveDirection.x * RotationRadius;
        Debug.Log("posX =" + _posX);
        _posY = RotationCenter.position.y + _playerInput.MoveDirection.y * RotationRadius;
        Debug.Log("posY =" + _posY);
        _shipBooster.position = new Vector3(_posX, _posY, 0);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, _angle);
    }
}