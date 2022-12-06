using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingManager : MonoBehaviour
{
    #region Editor Fields

    [Header("Description Panel")]
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;
    [SerializeField] private Transform _ingredientsContentTransform;

    [Header("Items Panel")]
    [SerializeField] private GameObject _craftingPanel;
    [SerializeField] private Transform _contentTransform;
    [SerializeField] private GameObject _craftingItemUIPrefab;
    [SerializeField] private GameObject _itemUIPrefab;
    [SerializeField] private List<CraftingRecipy> _craftingRecipyList = new List<CraftingRecipy>();

    #endregion

    #region Private Variables

    private PlayerInputHandler _currentPlayer;
    private CraftingStation _currentCraftingStation;
    private static CraftingManager _instance;

    #endregion

    #region Public Properties

    public static CraftingManager Instance { get { return _instance; } }

    #endregion

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

    public void DisplayItemInfo(CraftingRecipy craftingRecipy)
    {
        _itemNameText.text = craftingRecipy.CraftedItem.Item.DisplayName;
        _itemDescriptionText.text = craftingRecipy.CraftedItem.Item.Description;

        LoadIngredients(craftingRecipy);
    }

    public void EnableCanvas(bool isEnabled, PlayerInputHandler currentPlayer, CraftingStation craftingStation)
    {
        _craftingPanel.SetActive(isEnabled);
        _currentPlayer = currentPlayer;
        _currentCraftingStation = craftingStation;

        DestroyItemsUI(_ingredientsContentTransform);

        if (isEnabled)
            LoadItems();
    }

    private void LoadIngredients(CraftingRecipy craftingRecipy)
    {
        DestroyItemsUI(_ingredientsContentTransform);

        foreach (ItemQuantity ingredient in craftingRecipy.CraftingIngredients)
        {
            GameObject itemUI = Instantiate(_itemUIPrefab, _ingredientsContentTransform);
            itemUI.GetComponent<ItemUI>().Initialize(ingredient, _currentPlayer);
        }
    }

    private void LoadItems()
    {
        DestroyItemsUI(_contentTransform);

        foreach (CraftingRecipy craftable in _craftingRecipyList)
        {
            GameObject itemUI = Instantiate(_craftingItemUIPrefab, _contentTransform);
            itemUI.GetComponent<CraftingItemUI>().Initialize(craftable, _currentPlayer, _currentCraftingStation);
        }
    }

    private void DestroyItemsUI(Transform contentTransform)
    {
        foreach (Transform child in contentTransform)
        {
            if (child == _contentTransform) { continue; }

            Destroy(child.gameObject);
        }
    }
}
