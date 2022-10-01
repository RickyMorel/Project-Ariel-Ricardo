using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class ShipCamera : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private float _shakeAmplitude;

    #endregion

    #region Private Variables

    private CinemachineBasicMultiChannelPerlin _virtualCameraNoise;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _virtualCameraNoise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    #endregion

    public void Shake()
    {

    }
}