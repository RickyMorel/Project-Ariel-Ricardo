using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealth : Damageable
{
    #region Private Varaibles

    #endregion

    public override void Start()
    {
        base.Start();

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

    }

    private void HandleDamaged()
    {

    }
}
