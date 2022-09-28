using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealth : Damageable
{
    #region Editor Fields

    [SerializeField] private Image _healthBarImage;

    #endregion

    #region Private Varaibles

    #endregion

    public override void Start()
    {
        base.Start();

        UpdateHealthUI();

        OnUpdateHealth += HandleUpdateHealth;
        OnDamaged += HandleDamaged;
    }

    private void OnDestroy()
    {
        OnUpdateHealth -= HandleUpdateHealth;
        OnDamaged -= HandleDamaged;
    }

    private void HandleUpdateHealth()
    {
        UpdateHealthUI();
    }

    private void HandleDamaged()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        _healthBarImage.fillAmount = CurrentHealth / MaxHealth;
    }
}
