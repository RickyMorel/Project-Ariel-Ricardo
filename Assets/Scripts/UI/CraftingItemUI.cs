using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingItemUI : ItemUI
{
    #region Private Variables

    [SerializeField] private CraftingRecipy _craftingRecipy;

    #endregion

    #region Public Properties

    public CraftingRecipy CraftingRecipy => _craftingRecipy;

    #endregion

    public override void Initialize(CraftingRecipy craftingRecipy, PlayerInputHandler currentPlayer)
    {
        _itemQuantity = craftingRecipy.CraftedItem;

        _icon.sprite = _itemQuantity.Item.Icon;
        _amountText.text = $"x{_itemQuantity.Amount}";

        _currentPlayer = currentPlayer;

        _craftingRecipy = craftingRecipy;
    }

    public override void OnClick()
    {
        CraftingManager.Instance.DisplayItemInfo(CraftingRecipy);
    }
}
