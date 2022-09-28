using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{
    #region Editor Fields

    [Header("FX")]
    [SerializeField] private ParticleSystem _hitParticles;

    #endregion

    #region Unity Loops

    public override void Start()
    {
        base.Start();

        OnDamaged += HandleDamaged;
    }

    private void OnDestroy()
    {
        OnDamaged -= HandleDamaged;
    }

    #endregion

    private void HandleDamaged()
    {
        _hitParticles.Play();
    }

    public override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }
}
