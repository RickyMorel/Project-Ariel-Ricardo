using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootItemUI : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemAmountText;

    #endregion

    public void Initialize(ItemQuantity itemQuantity)
    {
        _icon.sprite = itemQuantity.Item.Icon;
        _itemNameText.text = itemQuantity.Item.DisplayName;
        _itemAmountText.text = itemQuantity.Amount.ToString();
    }
}
