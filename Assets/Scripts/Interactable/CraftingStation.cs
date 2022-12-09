using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : Interactable
{
    #region Editor Fields

    [SerializeField] private Transform _itemSpawnTransform;

    #endregion

    #region Public Properties

    public static event Action OnCraft;

    #endregion

    public override void Awake()
    {
        base.Awake();

        OnInteract += HandleInteract;
        OnUninteract += HandleUnInteract;
    }

    private void OnDestroy()
    {
        OnInteract -= HandleInteract;
        OnUninteract -= HandleUnInteract;
    }

    public void TryCraft(CraftingRecipy craftingRecipy)
    {
        if (!CanCraft(craftingRecipy)) { return; }

        Craft(craftingRecipy, craftingRecipy.CraftingIngredients);
    }

    public bool CanCraft(CraftingRecipy craftingRecipy)
    {
        Dictionary<Item, ItemQuantity> ownedItems = MainInventory.Instance.InventoryDictionary;
        foreach (ItemQuantity itemQuantity in craftingRecipy.CraftingIngredients)
        {
            //if has item
            if (!ownedItems.TryGetValue(itemQuantity.Item, out ItemQuantity ownedItemQuantity)) { return false; }

            //if has correct amount
            if (ownedItemQuantity.Amount < itemQuantity.Amount) { return false; }
        }

        return true;
    }

    private void Craft(CraftingRecipy craftingRecipy, List<ItemQuantity> usedResources)
    {
        MainInventory.Instance.RemoveItems(usedResources);

        GameObject spawnedItem = craftingRecipy.CraftedItem.Item.SpawnItemPickup(_itemSpawnTransform);
        if(spawnedItem.TryGetComponent<ChipPickup>(out ChipPickup chipPickup)) { chipPickup.Initialize(craftingRecipy.CraftedItem.Item); }

        OnCraft?.Invoke();
    }

    private void HandleInteract()
    {
        if(_currentPlayer == null) { HandleUnInteract(); return; }

        CraftingManager.Instance.EnableCanvas(true, _currentPlayer.GetComponent<PlayerInputHandler>(), this);
    }

    private void HandleUnInteract()
    {
        CraftingManager.Instance.EnableCanvas(false, null, this);
    }
}
