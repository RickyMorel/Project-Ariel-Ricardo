using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTrigger : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private PlayerUpgradesController _upgradesController;
    [SerializeField] private PlayerCarryController _playerCarryController;

    #endregion

    #region Private Variables

    private List<ItemPrefab> _currentItems = new List<ItemPrefab>();
    private ChipPickup _currentChipPickup;

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
        CheckForUpgradeChip(other, true);
        CheckForItem(other, true);
    }


    private void OnTriggerExit(Collider other)
    {
        CheckForUpgradeChip(other, false);
        CheckForItem(other, true);
    }

    #endregion

    private void HandleInteract()
    {
        PickupUpgradeChip();

        PickupItem();
    }

    #region Upgrade Chips
    private void CheckForUpgradeChip(Collider other, bool isEnter)
    {
        if (other.gameObject.TryGetComponent<ChipPickup>(out ChipPickup chipPickup)) { _currentChipPickup = isEnter ? chipPickup : null; }
    }

    private void PickupUpgradeChip()
    {
        if (_currentChipPickup != null) { _currentChipPickup.PickUp(_upgradesController); }
    }

    #endregion

    #region Items

    private void CheckForItem(Collider other, bool isEnter)
    {
        if (!other.gameObject.TryGetComponent<ItemPrefab>(out ItemPrefab itemPrefab)) { return; }

        itemPrefab.EnableOutline(isEnter);

        if (isEnter) { _currentItems.Add(itemPrefab); }
        else { _currentItems.Remove(itemPrefab); }

    }

    private void PickupItem()
    {
        if (_currentItems.Count < 1) { return; }

        if (_currentItems[0] == null) { _currentItems.RemoveAt(0); return; }

        if (HasPickedUpItem(_currentItems[0].gameObject)) { _currentItems.RemoveAt(0); return; }

        _currentItems[0].PickUp(_playerCarryController);

        _currentItems.RemoveAt(0);
    }

    private bool HasPickedUpItem(GameObject wantedItem)
    {
        foreach (ItemPrefab carriedItem in _playerCarryController.ItemsCarrying)
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
