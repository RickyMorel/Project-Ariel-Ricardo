using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHealth : Damageable
{
    #region Private Variables

    private Interactable _interactable;
    private GameObject _currentParticles;

    #endregion

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }

    public void FixInteractable()
    {
        AddHealth((int)MaxHealth);

        if(_currentParticles != null) { Destroy(_currentParticles); }
    }

    public override void Die()
    {
        base.Die();

        _currentParticles = Instantiate(GameAssetsManager.Instance.InteractableFriedParticles, transform);
        _interactable.RemoveCurrentPlayer();
    }
}
