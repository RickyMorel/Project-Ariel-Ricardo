using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    private void Start()
    {
        OnInteract += HandleInteract;
        OnUninteract += HandleUninteract;
    }

    private void OnDestroy()
    {
        OnInteract -= HandleInteract;
        OnUninteract -= HandleUninteract;
    }

    private void HandleInteract()
    {
        MainInventory.Instance.EnableInventory(true);
    }

    private void HandleUninteract()
    {
        MainInventory.Instance.EnableInventory(false);
    }
}
