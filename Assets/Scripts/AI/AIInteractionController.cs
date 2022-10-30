using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInteractionController : BaseInteractionController
{
    #region Private Variables

    private GAgent _gAgent;

    #endregion

    public override void Start()
    {
        base.Start();

        _gAgent = GetComponent<GAgent>();

        _gAgent.OnDoAction += HandleInteraction;
        _gAgent.OnExitAction += CheckExitInteraction;
    }

    private void OnDestroy()
    {
        _gAgent.OnDoAction -= HandleInteraction;
        _gAgent.OnExitAction -= CheckExitInteraction;
    }
}
