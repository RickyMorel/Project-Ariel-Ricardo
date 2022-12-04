using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : PlayerHealth
{
    #region Editor Fields

    [SerializeField] private WeaknessType _weaknessType;

    #endregion

    #region Public Properties

    public WeaknessType WeaknessType => _weaknessType;

    #endregion

    #region Private Variables

    private GAgent _gAgent;
    private AIInteractionController _interactionController;

    #endregion

    #region Unity Loops

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
    public override void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<Projectile>(out Projectile projectile)) { return; }

        //Turrets can't harm their own ship
        if (other.gameObject.tag == "Untagged" && gameObject.tag == "MainShip") { return; }

        if (other.gameObject.tag == gameObject.tag) { return; }

        Damage(projectile.Damage);

        Destroy(projectile.gameObject);
    }

    #endregion

    public override void Damage(int damage)
    {

        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);

        UpdateHealthUI();

        OnDamaged?.Invoke();

        if (DamageParticles != null) { DamageParticles.Play(); }

        if (CurrentHealth == 0)
            Die();
    }

    public override void Hurt()
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
}
