using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingItemUI : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _amountText;

    #endregion

    #region Private Variables

    private ItemQuantity _itemQuantity;
    private PlayerInputHandler _currentPlayer;

    #endregion

    public void Initialize(ItemQuantity itemQuantity, PlayerInputHandler currentPlayer)
    {
        _icon.sprite = itemQuantity.Item.Icon;
        _amountText.text = $"x{itemQuantity.Amount}";

        _itemQuantity = itemQuantity;
        _currentPlayer = currentPlayer;
    }

    public void OnClick()
    {
        //ItemQuantitySliderUI.Instance.Initialize(_itemQuantity, _chest, _currentPlayer, transform.position);
    }
}
