using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AIHealth : PlayerHealth
{
    #region Editor Fields

    [SerializeField] private bool _canKill = false;

    #endregion

    #region Private Variables

    private GAgent _gAgent;
    private AIInteractionController _interactionController;
    private Rigidbody _rb;

    #endregion

    #region Public Properties

    public bool CanKill => _canKill;

    #endregion

    #region Unity Loops

    public override void Start()
    {
        base.Start();

        _gAgent = GetComponent<GAgent>();
        _interactionController = GetComponent<AIInteractionController>();
        _rb = GetComponent<Rigidbody>();

        OnDamaged += Hurt;
        OnDie += HandleDead;
    }

    private void OnDestroy()
    {
        OnDamaged -= Hurt;
        OnDie -= HandleDead;
    }

    #endregion

    private void HandleDead()
    {
        _gAgent.enabled = false;
        _rb.isKinematic = false;
        _rb.useGravity = true;

        Invoke(nameof(DestroySelf), 10f);
    }

    public override void Hurt(DamageType damageType)
    {
        if (CurrentHealth <= 0) { IsHurt = true; }

        CheckIfLowHealth();
        CheckIfScared();
    }

    private void CheckIfScared()
    {
        if (_gAgent.Beliefs.HasState("scared")) { return; }

        StopPreviousAction();

        _gAgent.Beliefs.AddState("scared", 1);
    }

    private void CheckIfLowHealth()
    {
        //If health is below 50%
        if (CurrentHealth > MaxHealth * 0.5f) { return; }

        if (_gAgent.Beliefs.HasState("hurt")) { return; }

        StopPreviousAction();

        _gAgent.Beliefs.AddState("hurt", 1);
    }

    private void StopPreviousAction()
    {
        _gAgent.CancelPreviousActions();
        _interactionController.CheckExitInteraction();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}