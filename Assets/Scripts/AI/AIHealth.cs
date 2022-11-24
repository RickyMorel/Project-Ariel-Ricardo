using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : PlayerHealth
{
    #region Private Variables

    private GAgent _gAgent;
    private AIInteractionController _interactionController;

    #endregion

    public override void Start()
    {
        base.Start();

        _gAgent = GetComponent<GAgent>();
        _interactionController = GetComponent<AIInteractionController>();

        OnDamaged += Hurt;
    }

    private void OnDestroy()
    {
        OnDamaged -= Hurt;
    }

    public override void Hurt()
    {
        _gAgent.CurrentAction?.PostPeform();
        _gAgent.CurrentAction = null;
        _interactionController.CheckExitInteraction();

        if(CurrentHealth <= 0) { IsHurt = true; }

        CheckIfLowHealth();
        CheckIfScared();
    }

    private void CheckIfScared()
    {
        if (_gAgent.Beliefs.HasState("scared")) { return; }

        _gAgent.Beliefs.AddState("scared", 1);
    }

    private void CheckIfLowHealth()
    {
        //If health is below 50%
        if(CurrentHealth > MaxHealth * 0.5f) { return; }

        if (_gAgent.Beliefs.HasState("hurt")) { return; }

        _gAgent.Beliefs.AddState("hurt", 1);
    }
}
