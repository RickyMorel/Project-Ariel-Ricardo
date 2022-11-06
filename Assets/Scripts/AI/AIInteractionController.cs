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

        _gAgent.OnExitAction += CheckExitInteraction;
    }

    private void OnDestroy()
    {
        _gAgent.OnExitAction -= CheckExitInteraction;
    }

    public override void SetCurrentInteractable(Interactable interactable)
    {
        base.SetCurrentInteractable(interactable);

        if(interactable == null) { return; }

        if(_gAgent.CurrentAction == null || _gAgent.CurrentAction.Target == null || _gAgent.CurrentAction.IsRunning == false) { return; }

        Interactable wantedInteractable = _gAgent?.CurrentAction?.Target?.GetComponent<Interactable>();

        if(interactable == wantedInteractable)
        {
            HandleInteraction();
        }
    }
}
