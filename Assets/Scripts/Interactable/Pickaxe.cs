using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : RotationalInteractable
{
    #region Private Variables

    private bool _isBoosting = false;

    #endregion

    #region Unity Loops

    public override void Update()
    {
        base.Update();

        if (_currentPlayer == null) { SetIsBoosting(false); return; }

        if (!_currentPlayer.IsUsing) { SetIsBoosting(false); return; }

        SetIsBoosting(true);
    }

    #endregion

    private void SetIsBoosting(bool isBoosting)
    {
        //If value is the same, don't update
        if (_isBoosting == isBoosting) { return; }

        _isBoosting = isBoosting;

        //OnBoostUpdated?.Invoke(isBoosting);

        //if (isBoosting)
        //    BoostImpulse();
    }
}
