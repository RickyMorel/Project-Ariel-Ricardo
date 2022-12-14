using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ItemPickup : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] protected Item _itemSO;

    #endregion

    #region Public Properties

    public Item ItemSO => _itemSO;
    public Rigidbody Rb => _rb;
    public PlayerCarryController PrevPlayerCarryController => _prevPlayerCarryController;

    #endregion

    #region Private Variables

    private Outline _outline;
    private Rigidbody _rb;
    private PlayerCarryController _prevPlayerCarryController = null;

    #endregion

    #region Unity Loops

    public virtual void Start()
    {
        _outline = GetComponent<Outline>();
        _rb = GetComponent<Rigidbody>();

        EnableOutline(false);
    }

    #endregion

    public virtual void Initialize(Item item)
    {

    }

    public void EnableOutline(bool isEnabled)
    {
        _outline.enabled = isEnabled;
    }

    public void PickUp(PlayerCarryController playerCarryController)
    {
        _prevPlayerCarryController = playerCarryController;
        playerCarryController.CarryItem(this);
    }

    public void PickUpSingle(PlayerCarryController playerCarryController)
    {
        _prevPlayerCarryController = playerCarryController;
        playerCarryController.CarrySingle(this);
    }
}
