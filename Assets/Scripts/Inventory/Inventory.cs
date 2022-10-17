using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Transform _contentTransform;
    [SerializeField] private GameObject _inventoryItemUIPrefab;
    //This is temporary while we don't have an inventory system
    [SerializeField] private List<ItemQuantity> _preloadedItems = new List<ItemQuantity>();
    //

    #endregion

    #region Private Variables

    private Dictionary<Item, ItemQuantity> _inventory = new Dictionary<Item, ItemQuantity>();

    #endregion

    #region Public Properties

    public Dictionary<Item, ItemQuantity> InventoryDictionary => _inventory;

    #endregion


    #region Unity Loops

    //This is temporary while we don't have an inventory system
    private void Start()
    {
        AddItems(_preloadedItems);
    }
    //

    #endregion

    #region UI

    public void EnableInventory(bool isEnabled)
    {
        _inventoryPanel.SetActive(isEnabled);

        if (isEnabled)
            LoadItems();
    }

    private void LoadItems()
    {
        DestroyItemsUI();

        foreach (KeyValuePair<Item, ItemQuantity> item in _inventory)
        {
            GameObject itemUI = Instantiate(_inventoryItemUIPrefab, _contentTransform);
            itemUI.GetComponent<InventoryItemUI>().Initialize(item.Value);
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
            if (_inventory.ContainsKey(itemQuantity.Item))
            {
                _inventory[itemQuantity.Item].Amount += itemQuantity.Amount;
            }
            else
            {
                _inventory.Add(itemQuantity.Item, itemQuantity);
            }
        }
    }

    public void RemoveItems(List<ItemQuantity> removedItems)
    {
        foreach (ItemQuantity itemQuantity in removedItems)
        {
            if (!_inventory.ContainsKey(itemQuantity.Item)) { Debug.LogError("TRYING TO REMOVE ITEM THAT DOESN'T EXIST: " + itemQuantity.Item); continue; }

            _inventory[itemQuantity.Item].Amount -= itemQuantity.Amount;

            if (_inventory[itemQuantity.Item].Amount < 1)
                _inventory.Remove(itemQuantity.Item);
        }
    }
}
