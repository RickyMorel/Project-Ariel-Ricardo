using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLandingController : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Transform _landingGearTransform;
    [SerializeField] private float _landingGearStoredYPos;
    [SerializeField] private float _landingGearDeployedYPos;

    #endregion

    #region Private Variables

    [SerializeField] private bool _isWantedDeployed = false;
    [SerializeField] private bool _isLandingGearDeployed = false;

    #endregion

    private void FixedUpdate()
    {
        if(_isWantedDeployed == _isLandingGearDeployed) { return; }

        float wantedYPosition = _isLandingGearDeployed == true ? _landingGearStoredYPos : _landingGearDeployedYPos;

        Debug.Log("wantedPosition: " + wantedYPosition);

        _landingGearTransform.localPosition = new 
            Vector3(_landingGearTransform.localPosition.x, 
            Mathf.Lerp(_landingGearTransform.localPosition.y, wantedYPosition, Time.deltaTime), _landingGearTransform.localPosition.z);

        if(Mathf.RoundToInt(_landingGearTransform.localPosition.y) == Mathf.RoundToInt(wantedYPosition)) { _isLandingGearDeployed = _isWantedDeployed; }
    }
}
