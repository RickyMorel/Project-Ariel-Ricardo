using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemUI : ItemUI
{
    #region Private Variables

    private Chest _chest;

    #endregion

    public override void Initialize(ItemQuantity itemQuantity, Chest chest, PlayerInputHandler currentPlayer)
    {
        _icon.sprite = itemQuantity.Item.Icon;
        _amountText.text = $"x{itemQuantity.Amount}";

        _itemQuantity = itemQuantity;
        _chest = chest;
        _currentPlayer = currentPlayer;
    }

    public override void OnClick()
    {
        ItemQuantitySliderUI.Instance.Initialize(_itemQuantity, _chest, _currentPlayer, transform.position);
    }
}
