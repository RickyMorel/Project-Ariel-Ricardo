using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Transform _contentTransform;
    [SerializeField] private GameObject _inventoryItemUIPrefab;

    #endregion

    #region Private Variables

    private Dictionary<Item, ItemQuantity> _inventory = new Dictionary<Item, ItemQuantity>();
    private Chest _currentChest;
    private PlayerInputHandler _currentPlayer;

    #endregion

    #region Public Properties

    public Dictionary<Item, ItemQuantity> InventoryDictionary => _inventory;

    #endregion


    #region UI

    public void EnableInventory(bool isEnabled, Chest chest, PlayerInputHandler currentPlayer)
    {
        _inventoryPanel.SetActive(isEnabled);
        _currentChest = chest;
        _currentPlayer = currentPlayer;

        if (isEnabled)
            LoadItems();
    }

    private void LoadItems()
    {
        DestroyItemsUI();

        foreach (KeyValuePair<Item, ItemQuantity> item in _inventory)
        {
            GameObject itemUI = Instantiate(_inventoryItemUIPrefab, _contentTransform);
            itemUI.GetComponent<InventoryItemUI>().Initialize(item.Value, _currentChest, _currentPlayer);
        }
    }

    private void DestroyItemsUI()
    {
        foreach (Transform child in _contentTransform)
        {
            if(child == _contentTransform) { continue; }

            Destroy(child.gameObject);
        }
    }

    #endregion

    public virtual void AddItems(List<ItemQuantity> addedItems)
    {
        foreach (ItemQuantity itemQuantity in addedItems)
        {
            AddItem(itemQuantity);
        }
    }

    public void AddItem(ItemQuantity itemQuantity)
    {
        if (_inventory.ContainsKey(itemQuantity.Item))
        {
            _inventory[itemQuantity.Item].Amount += itemQuantity.Amount;
        }
        else
        {
            _inventory.Add(itemQuantity.Item, itemQuantity);
        }
    }

    public void RemoveItems(List<ItemQuantity> removedItems)
    {
        foreach (ItemQuantity itemQuantity in removedItems)
        {
            RemoveItem(itemQuantity);
        }
    }

    public void RemoveItem(ItemQuantity itemQuantity)
    {
        if (!_inventory.ContainsKey(itemQuantity.Item)) { Debug.LogError("TRYING TO REMOVE ITEM THAT DOESN'T EXIST: " + itemQuantity.Item); return; }

        _inventory[itemQuantity.Item].Amount -= itemQuantity.Amount;

        if (_inventory[itemQuantity.Item].Amount < 1)
            _inventory.Remove(itemQuantity.Item);

        LoadItems();
    }
}
