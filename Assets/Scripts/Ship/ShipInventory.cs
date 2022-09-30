using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInventory : MonoBehaviour
{
    #region Editor Fields

    //This is temporary while we don't have an inventory system
    [SerializeField] private List<ItemQuantity> _preloadedItems = new List<ItemQuantity>();
    //

    #endregion

    #region Private Variables

    private Dictionary<Item, ItemQuantity> _inventory = new Dictionary<Item, ItemQuantity>();

    #endregion

    #region Public Properties

    public Dictionary<Item, ItemQuantity> Inventory => _inventory;

    #endregion

    //This is temporary while we don't have an inventory system
    private void Start()
    {
        foreach (ItemQuantity itemQuantity in _preloadedItems)
        {
            _inventory.Add(itemQuantity.Item, itemQuantity);
        }
    }
    //

    public void AddItems(List<ItemQuantity> addedItems)
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

            Debug.Log("Added Item: " + itemQuantity.Item);
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
