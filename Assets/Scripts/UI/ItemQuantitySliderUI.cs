using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ItemQuantitySliderUI : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private GameObject _itemQuantitySliderPanel;
    [SerializeField] private TextMeshProUGUI _currentAmountText;
    [SerializeField] private TextMeshProUGUI _totalAmountText;
    [SerializeField] private Slider _slider;

    #endregion

    #region Private Variables

    private static ItemQuantitySliderUI _instance;
    private int _currentAmount;
    private Chest _currentChest;
    private ItemQuantity _finalItemQuantity = new ItemQuantity();

    #endregion

    #region Public Properties

    public static ItemQuantitySliderUI Instance { get { return _instance; } }

    #endregion

    #region Unity Loops

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        _slider.onValueChanged.AddListener(delegate { HandleSliderMoved(); });
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(delegate { HandleSliderMoved(); });
    }

    #endregion

    public void EnableSlider(bool isEnabled)
    {
        _itemQuantitySliderPanel.SetActive(isEnabled);
    }

    public void Initialize(ItemQuantity itemQuantity, Chest chest, Vector3 cellPosition)
    {
        transform.position = cellPosition;
        _finalItemQuantity.Item = itemQuantity.Item;
        _currentChest = chest;

        _slider.minValue = 0;
        _slider.maxValue = _currentAmount;
        _currentAmountText.text = _slider.value.ToString();
        _totalAmountText.text = itemQuantity.Amount.ToString();

        EnableSlider(true);
    }

    private void HandleSliderMoved()
    {
        if(_finalItemQuantity.Item == null) { return; }

        _currentAmount = (int)_slider.value;
        _finalItemQuantity.Amount = _currentAmount;
        Debug.Log("Current Amount: " + _currentAmount);
    }

    private void OnConfirm()
    {
        _currentChest.SpawnItem(_finalItemQuantity);
    }

}
