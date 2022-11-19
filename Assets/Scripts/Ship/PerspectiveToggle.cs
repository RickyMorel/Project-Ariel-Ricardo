using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveToggle : MonoBehaviour
{
    #region private Variables

    private CameraManager _cameraManager;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _cameraManager = FindObjectOfType<CameraManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ship>() == null) { return; }

        _cameraManager.CullingMaskToggle(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Ship>() == null) { return; }

        _cameraManager.CullingMaskToggle(false);
    }

    #endregion
}