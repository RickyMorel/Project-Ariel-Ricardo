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

    #region Public Properties

    public InteractionType InteractionType => _interactionType;
    public Transform PlayerPositionTransform => _playerPositionTransform;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _outline = GetComponent<Outline>();
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

    #endregion
}
