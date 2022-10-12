using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDoor : Interactable
{
    #region Editor Fields

    [SerializeField] private Transform _shipDoor;
    [SerializeField] private float _closedZRotation;
    [SerializeField] private float _openZRotation;

    #endregion

    #region Private Variables

    private bool _isWantedDoorOpen = false;
    private bool _isDoorOpen = false;
    private float _doorZRotation;

    #endregion

    private void Start()
    {
        OnInteract += HandleButtonPress;
    }

    private void OnDestroy()
    {
        OnInteract -= HandleButtonPress;
    }

    private void FixedUpdate()
    {
        if (_isWantedDoorOpen == _isDoorOpen) { return; }

        float wantedZPosition = _isDoorOpen == true ? _closedZRotation : _openZRotation;

        _doorZRotation = Mathf.Lerp(_shipDoor.localEulerAngles.z, wantedZPosition, Time.deltaTime);

        _shipDoor.localEulerAngles = new Vector3(_shipDoor.localEulerAngles.x, _shipDoor.localEulerAngles.y, _doorZRotation);

        if (Mathf.RoundToInt(_shipDoor.localEulerAngles.z) == Mathf.RoundToInt(wantedZPosition)) { _isDoorOpen = _isWantedDoorOpen; }
    }

    private void HandleButtonPress()
    {
        if (_isWantedDoorOpen != _isDoorOpen) { return; }

        _isWantedDoorOpen = !_isWantedDoorOpen;
    }
}
