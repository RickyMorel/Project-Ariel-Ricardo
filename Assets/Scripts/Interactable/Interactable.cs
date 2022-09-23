using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public abstract class Interactable : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private InteractionType _interactionType;
    [SerializeField] private Transform _playerPositionTransform;

    #endregion

    #region Private Variables

    private Outline _outline;

    #endregion

    #region Protected Variables

    protected PlayerInputHandler _currentPlayer;

    #endregion

    #region Public Properties

    public InteractionType InteractionType => _interactionType;
    public Transform PlayerPositionTransform => _playerPositionTransform;
    public PlayerInputHandler CurrentPlayer => _currentPlayer;

    #endregion

    #region Unity Loops

    private void Awake()
    {
        _outline = GetComponent<Outline>();

        _outline.enabled = false;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<PlayerInteractionController>(out PlayerInteractionController playerInteractionController)) { return; }

        _outline.enabled = true;

        playerInteractionController.SetCurrentInteractable(this);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerInputHandler>() == null) { return; }

        _outline.enabled = false;
    }

    public void SetCurrentPlayer(PlayerInputHandler playerInput)
    {
        _currentPlayer = playerInput;
    }

    public virtual void UseInteractable()
    {
        Debug.Log("Use Interactable");
    }

    #endregion
}
