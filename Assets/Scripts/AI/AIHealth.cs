using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : PlayerHealth
{
    #region Private Variables

    private GAgent _gAgent;

    #endregion

    public override void Start()
    {
        base.Start();

        _gAgent = GetComponent<GAgent>();
    }

    public override void Hurt()
    {
        base.Hurt();

        _gAgent.Beliefs.AddState("scared", 1);
    }
}
