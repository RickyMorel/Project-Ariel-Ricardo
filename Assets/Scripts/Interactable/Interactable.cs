using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Interactable : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private InteractionType _interactionType;
    [SerializeField] private Transform _playerPositionTransform;
    [SerializeField] private bool _isAIOnlyInteractable = false;

    [Header("One Time Interaction Parameters")]
    [SerializeField] private bool _isSingleUse = false;
    [SerializeField] private float _singleUseTime = 0.5f;

    #endregion

    #region Private Variables

    private Outline _outline;

    #endregion

    #region Protected Variables

    protected BaseInteractionController _currentPlayer;

    #endregion

    #region Public Properties

    public InteractionType InteractionType => _interactionType;
    public Transform PlayerPositionTransform => _playerPositionTransform;
    public BaseInteractionController CurrentPlayer => _currentPlayer;
    public bool IsSingleUse => _isSingleUse;
    public float SingleUseTime => _singleUseTime;

    public event Action OnInteract;
    public event Action OnUninteract;

    #endregion

    #region Unity Loops

    public virtual void Awake()
    {
        _outline = GetComponent<Outline>();

        _outline.enabled = false;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER: " + gameObject.name);

        if (!other.gameObject.TryGetComponent<BaseInteractionController>(out BaseInteractionController interactionController)) { return; }

        if (interactionController is PlayerInteractionController) 
        {
            if (_isAIOnlyInteractable) { return; }

            _outline.enabled = true; 
        }

        Debug.Log("SetCurrentInteractable " + gameObject.name);

        interactionController.SetCurrentInteractable(this);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent<BaseInteractionController>(out BaseInteractionController interactionController)) { return; }

        if (interactionController is PlayerInteractionController) 
        {
            if (_isAIOnlyInteractable) { return; }

            _outline.enabled = false; 
        }

        interactionController.SetCurrentInteractable(null);
    }

    public void SetCurrentPlayer(BaseInteractionController interactionController)
    {
        _currentPlayer = interactionController;

        OnInteract?.Invoke();
    }

    public void Uninteract()
    {
        OnUninteract?.Invoke();
    }

    #endregion
}
