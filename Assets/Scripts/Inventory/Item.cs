using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public Sprite Icon;
    public virtual string DisplayName { get; protected set; }
    public string Description;
    public int Value;
    public GameObject ItemPrefab;
    public GameObject ItemPickupPrefab;
    public int ItemSize;
    public bool IsSingleHold = false;
}
