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
        MainInventory.Instance.EnableInventory(true, this, _currentPlayer);
    }

    private void HandleUninteract()
    {
        MainInventory.Instance.EnableInventory(false, null, null);
    }

    public void SpawnItem(ItemQuantity itemQuantity)
    {
        Debug.Log("SpawnItem: " + itemQuantity.Amount);
        for (int i = 0; i < itemQuantity.Amount; i++)
        {
            GameObject itemInstance = Instantiate(itemQuantity.Item.ItemPrefab, _itemSpawnTransform.position, Quaternion.identity);
        }

        MainInventory.Instance.RemoveItem(itemQuantity);
    }

    public void StoreItem(Collider other)
    {
        if (!other.gameObject.TryGetComponent<ItemPrefab>(out ItemPrefab itemPrefab)) { return; }

        StartCoroutine(StoreItemCoroutine(itemPrefab));
    }

    //Prevents from player dropping items while entering trigger, and duplication glitches
    private IEnumerator StoreItemCoroutine(ItemPrefab itemPrefab)
    {
        yield return new WaitForSeconds(1f);

        itemPrefab.PrevPlayerCarryController?.DropAllItems();

        ItemQuantity itemQuantity = new ItemQuantity();
        itemQuantity.Item = itemPrefab.ItemSO;
        itemQuantity.Amount = 1;
        MainInventory.Instance.AddItem(itemQuantity);

        Destroy(itemPrefab.gameObject);
    }
}
