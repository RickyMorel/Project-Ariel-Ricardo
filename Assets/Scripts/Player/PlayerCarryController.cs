using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarryController : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private List<GameObject> _itemsCarrying;
    [SerializeField] private GameObject _carryBoxCollider;
    [SerializeField] private Transform[] _itemSpawnTransforms;

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
    }

    private void OnDestroy()
    {
        OnItemsUpdate -= HandleItemsUpdate;
    }

    #endregion

    public void CarryItem(ItemPrefab itemObj)
    {
        itemObj.transform.parent = _carryBoxCollider.transform;
        GetRandomCarryPos(itemObj);

        _itemsCarrying.Add(itemObj.gameObject);

        OnItemsUpdate?.Invoke();
    }

    private void GetRandomCarryPos(ItemPrefab itemObj)
    {
        int randomSpawnLocation = UnityEngine.Random.Range(0, _itemSpawnTransforms.Length);

        Vector3 spawnPos = _itemSpawnTransforms[randomSpawnLocation].localPosition;
        itemObj.transform.localPosition = spawnPos;
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
