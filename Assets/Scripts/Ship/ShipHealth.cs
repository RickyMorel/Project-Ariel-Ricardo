using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealth : Damageable
{
    #region Editor Fields

    [SerializeField] private InteractableHealth _boosterHealth;
    [SerializeField] private LayerMask _crashLayers;
    [SerializeField] private float _minCrashSpeed = 20f;
    [SerializeField] private float _crashDamageMultiplier = 10f;
    [SerializeField] private ParticleSystem _shipCrashParticles;
    #endregion

    #region Private Varaibles

    private Rigidbody _rb;
    private float _currentDamage;

    #endregion

    #region Unity Loops

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _boosterHealth.SetMaxHealth((int)MaxHealth);
    }

    public override void Start()
    {
        base.Start();

        OnUpdateHealth += HandleUpdateHealth;
        OnDamaged += HandleDamaged;
        _boosterHealth.OnFix += HandleFix;
    }

    private void OnDestroy()
    {
        OnUpdateHealth -= HandleUpdateHealth;
        OnDamaged -= HandleDamaged;
        _boosterHealth.OnFix -= HandleFix;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if((1<<other.gameObject.layer & _crashLayers) == 0) { return; }
        if(other.gameObject.GetComponent<Projectile>() != null) { return; }
        if(_rb.velocity.magnitude < _minCrashSpeed) { return; }

        _currentDamage = (int)CalculateCrashDamage();
        Damage((int)_currentDamage, DamageType.Base);
        if (other.TryGetComponent<AIHealth>(out AIHealth enemyHealth)) { enemyHealth.Damage((int)_currentDamage); }

        float currentSpeedPercentage = _rb.velocity.magnitude / Ship.Instance.TopSpeed;
        float crashImpactPercentageRatio = 4 * currentSpeedPercentage;
        float impactAmplitude = 5f * crashImpactPercentageRatio;
        ShipCamera.Instance.ShakeCamera(impactAmplitude, 50f, 0.2f);


        Vector3 hitPos = other.ClosestPointOnBounds(transform.position);
        _shipCrashParticles.transform.position = hitPos;
        _shipCrashParticles.Play();
    }

    #endregion

    private void HandleFix()
    {
        AddHealth((int)MaxHealth);
    }

    private float CalculateCrashDamage()
    {
        float finalDamage = _rb.velocity.magnitude* _crashDamageMultiplier;

        return finalDamage;
    }

    private void HandleUpdateHealth(int healthAdded)
    {
        Debug.Log("CurrentHealth: " + CurrentHealth);
        _boosterHealth.SetHealth((int)CurrentHealth);
    }

    private void HandleDamaged(DamageType damageType)
    {
        Debug.Log("CurrentHealth: " + CurrentHealth);
        _boosterHealth.SetHealth((int)CurrentHealth);
    }
}
