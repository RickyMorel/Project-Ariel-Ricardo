using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _amountText;

    #endregion

    #region Private Variables

    private ItemQuantity _itemQuantity;
    private Chest _chest;

    #endregion

    public void Initialize(ItemQuantity itemQuantity, Chest chest)
    {
        _icon.sprite = itemQuantity.Item.Icon;
        _amountText.text = $"x{itemQuantity.Amount}";

        _itemQuantity = itemQuantity;
        _chest = chest;
    }

    public void OnClick()
    {
        ItemQuantitySliderUI.Instance.Initialize(_itemQuantity, _chest, transform.position);
    }
}
