using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    #region Editor Fields

    [SerializeField] private Transform _itemSpawnTransform;

    #endregion

    #region Unity Loops

    private void Start()
    {
        OnInteract += HandleInteract;
        OnUninteract += HandleUninteract;
    }

    private void OnDestroy()
    {
        OnInteract -= HandleInteract;
        OnUninteract -= HandleUninteract;
    }

    #endregion

    private void HandleInteract()
    {
        MainInventory.Instance.EnableInventory(true, this);
    }

    private void HandleUninteract()
    {
        MainInventory.Instance.EnableInventory(false, null);
    }

    public void SpawnItem(ItemQuantity itemQuantity)
    {
        for (int i = 0; i < itemQuantity.Amount; i++)
        {
            Debug.Log("INSTANTIATE ITEM");
            GameObject itemInstance = Instantiate(itemQuantity.Item.ItemPrefab, _itemSpawnTransform.position, Quaternion.identity);
        }

        MainInventory.Instance.RemoveItem(itemQuantity);
    }

    public void StoreItem()
    {

    }
}
