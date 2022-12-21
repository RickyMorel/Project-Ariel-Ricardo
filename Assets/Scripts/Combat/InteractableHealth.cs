using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHealth : Damageable
{
    #region Editor Fields

    [SerializeField] private CraftingRecipy _fixCost;
    [SerializeField] private float _timeToFix = 8f;

    #endregion

    #region Private Variables

    private Interactable _interactable;
    private GameObject _currentParticles;
    private float _timeSpentFixing = 0f;
    private PrevInteractableState _prevInteractableState;
    private GameObject _currentRepairPopup;
    private GameObject _currentRepairCanvas;

    #endregion

    #region Unity Loops

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();

        _interactable.OnInteract += TryStartFix;
        _interactable.OnUninteract += TryStopFix;
    }

    private void OnDestroy()
    {
        _interactable.OnInteract -= TryStartFix;
        _interactable.OnUninteract -= TryStopFix;
    }

    private void Update()
    {
        if(_interactable.CurrentPlayer == null) { return; }

        if (!IsDead()) { return; }

        if (!CraftingManager.CanCraft(_fixCost)) { _interactable.RemoveCurrentPlayer(); return; }

        _timeSpentFixing += Time.deltaTime;

        if(_timeSpentFixing > _timeToFix) { FixInteractable(); }
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(_interactable.Outline.enabled == false) { return; }

        if(_currentRepairCanvas != null) { return; }

        if (!IsDead()) { return; }

        _currentRepairCanvas = RepairCostUI.Create(transform, _interactable.PlayerPositionTransform.localPosition, _fixCost).gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_interactable.Outline.enabled == true) { return; }

        if(_currentRepairCanvas != null) { Destroy(_currentRepairCanvas); }
    }

    #endregion

    private void TryStartFix()
    {
        if (!IsDead()) { return; }

        if (_currentRepairPopup != null) { Destroy(_currentRepairPopup); }

        if (!CraftingManager.CanCraft(_fixCost)) { return; }

        _timeSpentFixing = 0f;

        _currentRepairPopup = RepairPopup.Create(transform, _interactable.PlayerPositionTransform.localPosition, _timeToFix - _timeSpentFixing).gameObject;
    }

    private void TryStopFix()
    {
        if (_currentRepairPopup != null) { Destroy(_currentRepairPopup); }
    }

    public void FixInteractable()
    {
        AddHealth((int)MaxHealth);

        _interactable.CanUse = true;
        _interactable.InteractionType = _prevInteractableState.InteractionType;
        _interactable.IsSingleUse = _prevInteractableState.IsSingleUse;
        _interactable.Outline.OutlineColor = _prevInteractableState.OutlineColor;
        _timeSpentFixing = 0f;
        _interactable.RemoveCurrentPlayer();
        MainInventory.Instance.RemoveItems(_fixCost.CraftingIngredients);

        if (_currentRepairCanvas != null) { Destroy(_currentRepairCanvas); }
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

        _prevInteractableState = new PrevInteractableState(_interactable.InteractionType, _interactable.IsSingleUse, _interactable.Outline.OutlineColor);
        _interactable.InteractionType = InteractionType.Fixing;
        _interactable.IsSingleUse = false;
        _interactable.Outline.OutlineColor = Color.red;
    }

    #region Helper Classes

    private class PrevInteractableState
    {
        public PrevInteractableState(InteractionType originalInteractionType, bool originalIsSingleUse, Color outlineColor)
        {
            InteractionType = originalInteractionType;
            IsSingleUse = originalIsSingleUse;
            OutlineColor = outlineColor;
        }

        public InteractionType InteractionType;
        public bool IsSingleUse;
        public Color OutlineColor;
    }

    #endregion
}
