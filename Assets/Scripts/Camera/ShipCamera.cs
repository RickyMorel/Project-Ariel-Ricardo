using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class ShipCamera : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private float _shakeAmplitude;
    [SerializeField] private float _expandedBoostFOV = 17f;

    #endregion

    #region Private Variables

    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _virtualCameraNoise;

    private bool _isBoosting;
    private float _currentFOV;
    private float _orginalFOV;
    #endregion

    #region Unity Loops

    private void Awake()
    {
        Booster.OnBoostUpdated += HandleBoost;
    }

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _virtualCameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _orginalFOV = _virtualCamera.m_Lens.OrthographicSize;
        _currentFOV = _orginalFOV;
    }

    private void Update()
    {
        UpdateBoostFOVEffect();
    }

    private void OnDestroy()
    {
        Booster.OnBoostUpdated -= HandleBoost;
    }

    #endregion

    private void UpdateBoostFOVEffect()
    {
        float wantedFOV = _isBoosting == true ? _expandedBoostFOV : _orginalFOV;

        _currentFOV = Mathf.Lerp(_currentFOV, wantedFOV, Time.deltaTime);
        _virtualCamera.m_Lens.OrthographicSize = _currentFOV;
    }

    private void HandleBoost(bool isBoosting)
    {
        _isBoosting = isBoosting;

        float shakeAmount = isBoosting == true ? _shakeAmplitude : 0f;

        Shake(shakeAmount);
    }

    private void Shake(float shakeAmount)
    {
        _virtualCameraNoise.m_AmplitudeGain = shakeAmount;
    }
}