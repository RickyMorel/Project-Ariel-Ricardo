using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RepairPopup : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Color _normalTextColor;
    [SerializeField] private Color _criticalTextColor;

    #endregion

    #region Private Variables

    private TextMeshPro _timerText;
    private float _repairTime;

    #endregion

    #region Public Properties

    public static RepairPopup Create(Vector3 position, float repairDuration)
    {
        GameObject repairPopupObj = Instantiate(GameAssetsManager.Instance.RepairPopup, position, Quaternion.identity);
        RepairPopup repairPopup = repairPopupObj.GetComponent<RepairPopup>();
        repairPopup.Setup(repairDuration);

        return repairPopup;
    }

    #endregion

    private void Awake()
    {
        _timerText = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        _repairTime = Mathf.Clamp(_repairTime - Time.deltaTime, 0f, 999f);
        int repairTimeInt = (int)_repairTime;
        _timerText.text = repairTimeInt.ToString();

        if(_repairTime <= 0f) { Destroy(gameObject); }
    }

    public void Setup(float repairTime)
    {
        _repairTime = repairTime;
    }
}
