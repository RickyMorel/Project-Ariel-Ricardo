using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealth : Damageable
{
    #region Editor Fields

    [SerializeField] private LayerMask _crashLayers;
    [SerializeField] private float _minCrashSpeed = 20f;
    [SerializeField] private float _crashDamageMultiplier = 10f;
    [SerializeField] private ParticleSystem _shipCrashParticles;
    #endregion

    #region Private Varaibles

    private Rigidbody _rb;

    #endregion

    public override void Start()
    {
        base.Start();

        _rb = GetComponent<Rigidbody>();

        OnUpdateHealth += HandleUpdateHealth;
        OnDamaged += HandleDamaged;
    }

    private void OnDestroy()
    {
        OnUpdateHealth -= HandleUpdateHealth;
        OnDamaged -= HandleDamaged;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if((1<<other.gameObject.layer & _crashLayers) == 0) { return; }
        if(other.gameObject.GetComponent<Projectile>() != null) { return; }
        if(_rb.velocity.magnitude < _minCrashSpeed) { return; }

        Damage((int)CalculateCrashDamage());

        Vector3 hitPos = other.ClosestPointOnBounds(transform.position);
        _shipCrashParticles.transform.position = hitPos;
        _shipCrashParticles.Play();
        ShipCamera.Instance.ShakeCamera(5f, 50f, 0.2f);
    }

    private float CalculateCrashDamage()
    {
        float finalDamage = _rb.velocity.magnitude* _crashDamageMultiplier;

        return finalDamage;
    }

    private void HandleUpdateHealth()
    {

    }

    private void HandleDamaged()
    {

    }
}
