using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RepairCostUI : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Transform _contentTransform;

    #endregion

    #region Public Properties

    public static RepairCostUI Create(Transform interatableTransform, Vector3 localPos, CraftingRecipy craftingRecipy)
    {
        GameObject repairPopupObj = Instantiate(GameAssetsManager.Instance.RepairCostsCanvas, interatableTransform);
        repairPopupObj.transform.localPosition = new Vector3(localPos.x, 0f, localPos.z);

        RepairCostUI repairPopup = repairPopupObj.GetComponent<RepairCostUI>();
        repairPopup.Setup(craftingRecipy);

        return repairPopup;
    }

    #endregion

    public void Setup(CraftingRecipy craftingRecipy)
    {
        CraftingManager.LoadIngredients(craftingRecipy, _contentTransform);
    }
}
