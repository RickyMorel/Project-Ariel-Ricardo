using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private int _floorMask = 6;
    [SerializeField] private PlayerMovement _playerMovement;

    #endregion

    #region Private Variables

    #endregion

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer != _floorMask) { return; }

        _playerMovement.SetIsGrounded( true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != _floorMask) { return; }

        _playerMovement.SetIsGrounded(false);
    }
}
