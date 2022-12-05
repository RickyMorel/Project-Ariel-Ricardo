using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : Interactable
{
    public override void Awake()
    {
        base.Awake();

        OnInteract += HandleInteract;
        OnUninteract += HandleUnInteract;
    }

    private void OnDestroy()
    {
        OnInteract -= HandleInteract;
        OnUninteract -= HandleUnInteract;
    }

    private void HandleInteract()
    {
        CraftingManager.Instance.EnableCanvas(true, _currentPlayer.GetComponent<PlayerInputHandler>());
    }

    private void HandleUnInteract()
    {
        CraftingManager.Instance.EnableCanvas(false, null);
    }
}
