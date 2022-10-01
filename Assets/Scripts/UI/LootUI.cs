using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootUI : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private GameObject _lootItemUIPrefab;
    [SerializeField] private Transform _contentTransform;
    [SerializeField] private GameObject _lootPanel;

    #endregion

    #region Private Variables

    private static LootUI _instance;

    #endregion

    #region Public Properties

    public static LootUI Instance { get { return _instance; } }

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

    #endregion

    public void DisplayLootedItems(List<ItemQuantity> lootedItems)
    {
        DestroyPrevListedItems();

        foreach (ItemQuantity itemQuantity in lootedItems)
        {
            GameObject lootItemUI = Instantiate(_lootItemUIPrefab, _contentTransform);
            LootItemUI lootItem = lootItemUI.GetComponent<LootItemUI>();
            lootItem.Initialize(itemQuantity);
        }

        _lootPanel.SetActive(true);

        StartCoroutine(DisableLootPanel());
    }

    private IEnumerator DisableLootPanel()
    {
        yield return new WaitForSeconds(2f);

        _lootPanel.SetActive(false);
    }

    private void DestroyPrevListedItems()
    {
        foreach (Transform child in _contentTransform)
        {
            if(child == _contentTransform) { continue; }

            Destroy(child.gameObject);
        }
    }
}
