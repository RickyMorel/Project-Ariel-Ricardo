using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHealth : Damageable
{
    #region Editor Fields

    [SerializeField] private float _timeToFix = 8f;

    #endregion

    #region Private Variables

    private Interactable _interactable;
    private GameObject _currentParticles;
    private float _timeSpentFixing = 0f;
    private PrevInteractableState _prevInteractableState;

    #endregion

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }

    private void Update()
    {
        if(_interactable.CurrentPlayer == null) { return; }

        if (!IsDead()) { return; }

        _timeSpentFixing += Time.deltaTime;

        if(_timeSpentFixing > _timeToFix) { FixInteractable(); }
    }

    public void FixInteractable()
    {
        AddHealth((int)MaxHealth);

        _interactable.CanUse = true;
        _interactable.InteractionType = _prevInteractableState.InteractionType;
        _interactable.IsSingleUse = _prevInteractableState.IsSingleUse;
        _timeSpentFixing = 0f;

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

        _prevInteractableState = new PrevInteractableState(_interactable.InteractionType, _interactable.IsSingleUse);
        _interactable.InteractionType = InteractionType.Fixing;
        _interactable.IsSingleUse = false;
    }

    #region Helper Classes

    private class PrevInteractableState
    {
        public PrevInteractableState(InteractionType originalInteractionType, bool originalIsSingleUse)
        {
            InteractionType = originalInteractionType;
            IsSingleUse = originalIsSingleUse;
        }

        public InteractionType InteractionType;
        public bool IsSingleUse;
    }

    #endregion
}
