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

    private List<ItemPickup> _currentItems = new List<ItemPickup>();
    private ItemPickup _currentSingleItem;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _inputHandler.OnInteract += HandleInteract;
        _playerCarryController.OnDropAllItems += CheckIfItemsNotDestroyed;
    }

    private void OnDestroy()
    {
        _inputHandler.OnInteract -= HandleInteract;
        _playerCarryController.OnDropAllItems -= CheckIfItemsNotDestroyed;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForItem(other, true);
    }


    private void OnTriggerExit(Collider other)
    {
        CheckForItem(other, false);
    }

    #endregion

    private void HandleInteract()
    {
        PickupItem();
    }

    #region Items

    private void CheckForItem(Collider other, bool isEnter)
    {
        if (!other.gameObject.TryGetComponent<ItemPickup>(out ItemPickup itemPrefab)) { return; }

        itemPrefab.EnableOutline(isEnter);

        if (isEnter) 
        {
            if (itemPrefab.ItemSO.IsSingleHold) { _currentSingleItem = itemPrefab; }
            else { _currentItems.Add(itemPrefab); }
        }
        else 
        {
            if (itemPrefab.ItemSO.IsSingleHold) { _currentSingleItem = null; }
            else { _currentItems.Remove(itemPrefab); }
        }

    }

    private void PickupItem()
    {
        if (_currentSingleItem != null) { _currentSingleItem.PickUpSingle(_playerCarryController); return; }

        if (_currentItems.Count < 1) { return; }

        if (_currentItems[0] == null) { _currentItems.RemoveAt(0); return; }

        if (HasPickedUpItem(_currentItems[0].gameObject)) { _currentItems.RemoveAt(0); return; }

        _currentItems[0].PickUp(_playerCarryController);

        _currentItems.RemoveAt(0);
    }

    private bool HasPickedUpItem(GameObject wantedItem)
    {
        foreach (ItemPickup carriedItem in _playerCarryController.ItemsCarrying)
        {
            if (carriedItem.gameObject == wantedItem)
                return true;
        }

        return false;
    }

    private void CheckIfItemsNotDestroyed()
    {
        for (int i = _currentItems.Count - 1; i >= 0; i--)
        {
            if (_currentItems[i] != null) { continue; }

            _currentItems.RemoveAt(i);
        }
    }

    #endregion
}
