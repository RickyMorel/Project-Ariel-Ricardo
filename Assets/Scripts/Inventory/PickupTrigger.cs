using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTrigger : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private PlayerCarryController _playerCarryController;

    #endregion

    #region Private Variables

    private List<ItemPrefab> _currentItems = new List<ItemPrefab>();

    #endregion

    private void Start()
    {
        _inputHandler.OnInteract += HandleInteract;
    }

    private void OnDestroy()
    {
        _inputHandler.OnInteract -= HandleInteract;
    }

    private void HandleInteract()
    {
        if(_currentItems.Count < 1) { return; }

        _currentItems[0].PickUp(_playerCarryController);
    }

    #region Unity Loops

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.TryGetComponent<ItemPrefab>(out ItemPrefab itemPrefab)) { return; }

        itemPrefab.EnableOutline(true);

        _currentItems.Add(itemPrefab);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent<ItemPrefab>(out ItemPrefab itemPrefab)) { return; }

        itemPrefab.EnableOutline(false);

        _currentItems.Remove(itemPrefab);
    }

    #endregion
}
