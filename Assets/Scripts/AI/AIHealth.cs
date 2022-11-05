using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : PlayerHealth
{
    #region Private Variables

    private GAgent _gAgent;
    private AIInteractionController _interactionController;

    #endregion

    public override void Start()
    {
        base.Start();

        _gAgent = GetComponent<GAgent>();
        _interactionController = GetComponent<AIInteractionController>();
    }

    public override void Hurt()
    {
        base.Hurt();

        _gAgent.CurrentAction?.PostPeform();
        _gAgent.CurrentAction = null;
        _interactionController.CheckExitInteraction();

        if (_gAgent.Beliefs.HasState("scared")) { return; }

        _gAgent.Beliefs.AddState("scared", 1);
    }
}
