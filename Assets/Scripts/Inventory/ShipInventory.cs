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

    //This is temporary while we don't have an inventory system
    private void Start()
    {
        //foreach (Item itemQuantity in _preloadedItems)
        //{
        //    _inventory.Add(itemQuantity);
        //}
    }
    //
}
