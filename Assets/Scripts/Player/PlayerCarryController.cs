using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarryController : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private List<ItemPrefab> _itemsCarrying;
    [SerializeField] private GameObject _carryBoxCollider;
    [SerializeField] private Transform _armTransform;
    [SerializeField] private Transform[] _itemSpawnTransforms;

    #endregion

    #region Private Variables

    private PlayerInteractionController _interactionController;
    private Animator _anim;
    private bool _hasItems;
    private float _carryWalkSpeed = 1f;
    private float _maxCarryAmount = 10f;
    private float _itemCarryResetDistance = 0.5f;

    #endregion

    #region Public Properties

    public bool HasItems => _hasItems;
    public float CarryWalkSpeed => _carryWalkSpeed;
    public List<ItemPrefab> ItemsCarrying => _itemsCarrying;
    public event Action OnItemsUpdate;
    public event Action OnDropAllItems;

    #endregion

    #region Unity Loops

    private void Start()
    {
        OnItemsUpdate += HandleItemsUpdate;

        _anim = GetComponent<Animator>();
        _interactionController = GetComponent<PlayerInteractionController>();
    }

    private void FixedUpdate()
    {
        if(_carryBoxCollider == null) { return; }

        Vector3 boxPos = _carryBoxCollider.transform.position;
        float boxYOffset = 0.2f;
        float yDifference = _armTransform.position.y - boxPos.y + boxYOffset;

        _carryBoxCollider.transform.position = new Vector3(boxPos.x, boxPos.y + yDifference, boxPos.z);
    }

    private void OnDestroy()
    {
        OnItemsUpdate -= HandleItemsUpdate;
    }

    #endregion

    public void CarryItem(ItemPrefab itemObj)
    {
        if (_interactionController.CurrentInteractable != null) { return; }
        if (HasCarrySpace(itemObj.ItemSO) == false) { return; }

        itemObj.transform.parent = _carryBoxCollider.transform;
        GetRandomCarryPos(itemObj);
        StartCoroutine(EnableItemPhysics(itemObj));

        _itemsCarrying.Add(itemObj);

        OnItemsUpdate?.Invoke();
    }

    private IEnumerator EnableItemPhysics(ItemPrefab itemObj)
    {
        //Resets velocity to prevent spinning
        itemObj.Rb.velocity = Vector3.zero;
        itemObj.Rb.angularVelocity = Vector3.zero;
        itemObj.Rb.ResetInertiaTensor();

        itemObj.Rb.isKinematic = false;

        yield return new WaitForSeconds(0.5f);

        itemObj.Rb.isKinematic = true;

        //Resets item position if too far from hands
        if(Vector3.Distance(_carryBoxCollider.transform.position, itemObj.transform.position) > _itemCarryResetDistance)
        {
            itemObj.transform.localPosition = Vector3.zero;
            StartCoroutine(EnableItemPhysics(itemObj));
        }
    }

    private void GetRandomCarryPos(ItemPrefab itemObj)
    {
        int randomSpawnLocation = UnityEngine.Random.Range(0, _itemSpawnTransforms.Length);

        Vector3 spawnPos = _itemSpawnTransforms[randomSpawnLocation].localPosition;
        itemObj.transform.localPosition = spawnPos;
    }

    public void DropItem(ItemPrefab item)
    {
        _itemsCarrying.Remove(item);

        OnItemsUpdate?.Invoke();
    }

    public void DropAllItems()
    {
        foreach (ItemPrefab item in _itemsCarrying)
        {
            item.transform.parent = null;
            ItemPrefab itemPrefab = item.GetComponent<ItemPrefab>();
            itemPrefab.Rb.isKinematic = false;
        }

        _carryBoxCollider.SetActive(false);

        _itemsCarrying.Clear();

        OnItemsUpdate?.Invoke();

        OnDropAllItems?.Invoke();
    }

    public void DestroyAllItems()
    {
        foreach (ItemPrefab item in _itemsCarrying)
        {
            Destroy(item);
        }

        _carryBoxCollider.SetActive(false);

        _itemsCarrying.Clear();

        OnItemsUpdate?.Invoke();

        OnDropAllItems?.Invoke();
    }

    private void HandleItemsUpdate()
    {
        _hasItems = _itemsCarrying.Count > 0;

        _carryBoxCollider.SetActive(_hasItems);

        float carryAmount = 0;

        foreach (ItemPrefab item in _itemsCarrying)
        {
            carryAmount += item.ItemSO.ItemSize;
        }

        _anim.SetFloat("CarryAmount", carryAmount / _maxCarryAmount);
    }

    private bool HasCarrySpace(Item wantedItem)
    {
        float carryAmount = 0;

        foreach (ItemPrefab item in _itemsCarrying)
        {
            carryAmount += item.ItemSO.ItemSize;
        }

        return (carryAmount + wantedItem.ItemSize) <= _maxCarryAmount;
    }
}
