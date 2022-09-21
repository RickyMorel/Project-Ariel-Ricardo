using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradable : Interactable
{
    #region Editor Fields

    [SerializeField] private Upgrade[] _upgrades;
    [SerializeField] private Canvas _upgradesCanvas;

    #endregion

    #region Private Variables

    private int _currentLevel = 0;

    #endregion

    #region Unity Loops

    private void Start()
    {
        //TODO: read current level from save data

        EnableUpgradeMesh();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        _upgradesCanvas.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _upgradesCanvas.enabled = false;
    }

    #endregion

    public void TryUpgrade(Dictionary<Item, ItemQuantity> ownedItems)
    {
        foreach (ItemQuantity itemQuantity in _upgrades[_currentLevel].CraftingRecipy.CraftingIngredients)
        {
            //if has item
            if(!ownedItems.TryGetValue(itemQuantity.Item, out ItemQuantity ownedItemQuantity)) { return; }

            //if has correct amount
            if(ownedItemQuantity.Amount < itemQuantity.Amount) { return; }
        }

        Upgrade();
    }

    public void Upgrade()
    {
        _currentLevel++;

        EnableUpgradeMesh();
    }

    public void EnableUpgradeMesh()
    {
        foreach (Upgrade upgrade in _upgrades)
        {
            upgrade.UpgradeMesh.SetActive(false);
        }

        _upgrades[_currentLevel].UpgradeMesh.SetActive(true);
    }
}

#region Helper Classes

[System.Serializable]
public class Upgrade
{
    public GameObject UpgradeMesh;
    public CraftingRecipy CraftingRecipy;
}

#endregion
