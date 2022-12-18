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

        _interactable.CanUse = true;

        if (_currentParticles != null) { Destroy(_currentParticles); }
    }

    public override void Die()
    {
        base.Die();

        _currentParticles = Instantiate(GameAssetsManager.Instance.InteractableFriedParticles, transform);
        _currentParticles.transform.localPosition = new Vector3(
            _interactable.PlayerPositionTransform.localPosition.x,
                                                               0f,
            _interactable.PlayerPositionTransform.localPosition.z);

        _interactable.CanUse = false;

        _interactable.RemoveCurrentPlayer();
    }
}
