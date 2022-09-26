using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private int _currentInteraction = 0;

    #endregion

    #region Private Variables

    private PlayerInputHandler _playerInput;
    private Animator _anim;
    private Rigidbody _rb;
    private ShipInventory _shipInventory;
    private Interactable _currentInteractable;

    private float _timeSinceLastInteraction;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _playerInput = GetComponent<PlayerInputHandler>();
        _anim = GetComponent<Animator>();
        _shipInventory = FindObjectOfType<ShipInventory>();
        _rb = GetComponent<Rigidbody>();

        _playerInput.OnInteract += HandleInteraction;
        _playerInput.OnUpgrade += HandleUpgrade;
        _playerInput.OnJump += HandleJump;
    }

    private void Update()
    {
        _timeSinceLastInteraction += Time.deltaTime;
    }

    private void OnDestroy()
    {
        _playerInput.OnInteract -= HandleInteraction;
        _playerInput.OnUpgrade -= HandleUpgrade;
        _playerInput.OnJump -= HandleJump;
    }

    #endregion

    private void HandleJump()
    {
        CheckExitInteraction();
    }

    private void CheckExitInteraction()
    {
        //if is not doing interaction, return
        if (!IsInteracting()) { return; }

        //SetInteraction(0, transform);
    }

    //This calls when the player presses the interact button
    private void HandleInteraction()
    {
        if(_currentInteractable == null) { return; }

        if(_currentInteractable.CurrentPlayer == _playerInput) { return; }

        SetInteraction((int)_currentInteractable.InteractionType, _currentInteractable.PlayerPositionTransform);
    }

    //This calls when the player presses the upgrade button
    private void HandleUpgrade()
    {
        if (_currentInteractable == null) { return; }

        if((_currentInteractable is Upgradable) == false) { return; }

        Upgradable upgradable = _currentInteractable as Upgradable;
        upgradable.TryUpgrade(_shipInventory.Inventory);
    }

    public void SetCurrentInteractable(Interactable interactable)
    {
        _currentInteractable = interactable;
    }

    public bool IsInteracting()
    {
        return _currentInteraction != 0;
    }

    public bool HasRecentlyInteracted()
    {
        return _timeSinceLastInteraction < 0.25f;
    }

    public void SetInteraction(int interactionType, Transform playerPositionTransform)
    {
        PlayerInputHandler playerInput = interactionType == 0 ? null : _playerInput;
        _rb.isKinematic = interactionType != 0;

        _currentInteractable.SetCurrentPlayer(playerInput);
        _currentInteraction = interactionType;

        _anim.SetInteger("Interaction", interactionType);
        transform.position = playerPositionTransform.position;
        transform.rotation = playerPositionTransform.rotation;

        _timeSinceLastInteraction = 0f;
    }
}
