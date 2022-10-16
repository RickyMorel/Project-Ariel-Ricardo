using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ItemPrefab : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Item _itemSO;

    #endregion

    #region Public Properties

    public Item ItemSO => _itemSO;
    public Rigidbody Rb => _rb;

    #endregion

    #region Private Variables

    private Outline _outline;
    private Rigidbody _rb;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _outline = GetComponent<Outline>();
        _rb = GetComponent<Rigidbody>();

        EnableOutline(false);
    }

    #endregion

    public void EnableOutline(bool isEnabled)
    {
        _outline.enabled = isEnabled;
    }

    public void PickUp(PlayerCarryController playerCarryController)
    {
        playerCarryController.CarryItem(this);
    }
}
