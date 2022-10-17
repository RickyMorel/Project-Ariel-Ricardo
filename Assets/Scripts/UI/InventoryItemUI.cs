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

    public void Initialize(ItemQuantity itemQuantity)
    {
        _icon.sprite = itemQuantity.Item.Icon;
        _amountText.text = $"x{itemQuantity.Amount}";
    }

    public void OnClick()
    {

    }
}
