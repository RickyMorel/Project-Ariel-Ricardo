using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICombat : PlayerCombat
{
    private GAgent _gAgent;

    private void Start()
    {
        _gAgent = GetComponent<GAgent>();
    }

    public void Aggro()
    {
        if (_gAgent.Beliefs.HasState("aggro")) { return; }

        _gAgent.Beliefs.AddState("aggro", 1);
    }
}
