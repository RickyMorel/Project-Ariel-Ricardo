using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackShip : GAction
{
    #region Private Variables

    private AIStateMachine _stateMachine;
    private GAgent _gAgent;
    private AICombat _aiCombat;

    #endregion

    private void Start()
    {
        _gAgent = Agent.GetComponent<GAgent>();
        _stateMachine = Agent.GetComponent<AIStateMachine>();
        _aiCombat = Agent.GetComponent<AICombat>();
    }

    public override bool PrePerform()
    {
        Target = GWorld.Instance.GetQueue(GWorld.SHIP_ATTACK_POINTS).RemoveResource();

        if (Target == null) { return false; }

        Inventory.AddItem(Target);

        GWorld.Instance.GetWorld().ModifyState(GWorld.FREE_SHIP_ATTACK_POINTS, -1);

        _gAgent.SetGoalDistance(_aiCombat.AttackRange);

        return true;
    }

    public override bool PostPeform()
    {
        GWorld.Instance.GetQueue(GWorld.SHIP_ATTACK_POINTS).AddResource(Target);

        Inventory.RemoveItem(Target);

        GWorld.Instance.GetWorld().ModifyState(GWorld.FREE_SHIP_ATTACK_POINTS, 1);

        _stateMachine.BasicAttack();

        _gAgent.ResetGoalDistance();

        return true;
    }
}
