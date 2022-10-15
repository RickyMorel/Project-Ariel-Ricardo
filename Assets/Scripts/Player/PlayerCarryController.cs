using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarryController : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private List<ItemQuantity> _premadeList;
    [SerializeField] private List<GameObject> _itemsCarrying;
    [SerializeField] private GameObject _carryBoxCollider;
    [SerializeField] private Transform _itemSpawnTransform;

    #endregion

    #region Private Variables

    private Animator _anim;
    private bool _hasItems;
    private float _carryWalkSpeed = 1f;

    #endregion

    #region Public Properties

    public bool HasItems => _hasItems;
    public float CarryWalkSpeed => _carryWalkSpeed;
    public List<GameObject> ItemsCarrying => _itemsCarrying;
    public event Action OnItemsUpdate;

    #endregion

    #region Unity Loops

    private void Start()
    {
        OnItemsUpdate += HandleItemsUpdate;

        _anim = GetComponent<Animator>();

        Invoke(nameof(LateStart), 1f);
    }

    private void LateStart()
    {
        foreach (ItemQuantity item in _premadeList)
        {
            CarryItem(item.Item);
        }
    }

    private void OnDestroy()
    {
        OnItemsUpdate -= HandleItemsUpdate;
    }

    #endregion

    public void CarryItem(Item item)
    {
        GameObject itemInstance = Instantiate(item.ItemPrefab);
        itemInstance.transform.localPosition = _carryBoxCollider.transform.position;

        _itemsCarrying.Add(itemInstance);

        OnItemsUpdate?.Invoke();
    }

    public void DropItem(GameObject item)
    {
        _itemsCarrying.Remove(item);

        OnItemsUpdate?.Invoke();
    }

    public void DropAllItems()
    {
        _carryBoxCollider.SetActive(false);

        _itemsCarrying.Clear();

        OnItemsUpdate?.Invoke();
    }

    private void HandleItemsUpdate()
    {
        _hasItems = _itemsCarrying.Count > 0;

        _carryBoxCollider.SetActive(_hasItems);

        float maxItemAmount = 10f;

        _anim.SetFloat("CarryAmount", _itemsCarrying.Count / maxItemAmount);
    }
}
