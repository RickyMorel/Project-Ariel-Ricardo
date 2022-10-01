using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private List<ItemQuantity> _loot = new List<ItemQuantity>();
    [SerializeField] private bool _canLoot = false;

    #endregion

    #region Unity Loops

    private void OnTriggerEnter(Collider other)
    {
        if(_canLoot == false) { return; }

        if(_loot.Count < 1) { return; }

        if(!other.gameObject.TryGetComponent<ShipInventory>(out ShipInventory shipInventory)) { return; }

        shipInventory.AddItems(_loot);

        _loot.Clear();
    }

    #endregion

    public void SetCanLoot(bool canLoot)
    {
        _canLoot = canLoot;
    }
}
