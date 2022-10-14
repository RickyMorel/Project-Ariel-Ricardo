using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ItemPrefab : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Item _itemSO;

    #endregion

    #region Private Variables

    private Outline _outline;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _outline = GetComponent<Outline>();

        EnableOutline(false);
    }

    #endregion

    public void EnableOutline(bool isEnabled)
    {
        _outline.enabled = isEnabled;
    }

    public void PickUp(PlayerCarryController playerCarryController)
    {
        playerCarryController.CarryItem(_itemSO);

        Destroy(gameObject);
    }
}
