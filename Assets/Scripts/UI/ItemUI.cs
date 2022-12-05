using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] protected Image _icon;
    [SerializeField] protected TextMeshProUGUI _amountText;

    #endregion

    #region Private Variables

    protected ItemQuantity _itemQuantity;
    protected PlayerInputHandler _currentPlayer;

    #endregion

    public virtual void Initialize(ItemQuantity itemQuantity, PlayerInputHandler currentPlayer)
    {
        _icon.sprite = itemQuantity.Item.Icon;
        _amountText.text = $"x{itemQuantity.Amount}";

        _itemQuantity = itemQuantity;

        _currentPlayer = currentPlayer;
    }

    public virtual void Initialize(ItemQuantity itemQuantity, Chest chest, PlayerInputHandler currentPlayer) { }
    public virtual void Initialize(CraftingRecipy craftingRecipy, PlayerInputHandler currentPlayer) { }

    public virtual void OnClick()
    {

    }
}
