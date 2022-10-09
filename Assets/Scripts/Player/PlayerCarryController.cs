using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarryController : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Transform[] _carryTransforms;
    [SerializeField] private List<Item> _itemsCarrying;

    #endregion

    public void CarryItem(Item item)
    {
        _itemsCarrying.Add(item);

    }

    public void DropItem()
    {

    }
}
