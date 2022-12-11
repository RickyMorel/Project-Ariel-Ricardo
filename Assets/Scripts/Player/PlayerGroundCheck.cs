using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private int _floorMask = 6;
    [SerializeField] private int _shipFloorMask = 24;
    [SerializeField] private PlayerStateMachine _playerStateMachine;

    #endregion

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer != _floorMask && other.gameObject.layer != _shipFloorMask) { return; }

        Debug.Log("TOUCHING FLOOR");

        _playerStateMachine.IsGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != _floorMask && other.gameObject.layer != _shipFloorMask) { return; }

        Debug.Log("NOT TOUCHING FLOOR!");

        _playerStateMachine.IsGrounded = false;
    }
}