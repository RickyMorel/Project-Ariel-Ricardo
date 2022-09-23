using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public Sprite Icon;
    public string DisplayName;
    public string Description;
    public int Value;
}
