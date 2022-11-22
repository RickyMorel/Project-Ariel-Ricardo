using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackShip : GAction
{
    #region Private Variables

    private AIStateMachine _stateMachine;

    #endregion

    private void Start()
    {
        _stateMachine = Agent.GetComponent<AIStateMachine>();
    }

    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPeform()
    {
        _stateMachine.BasicAttack();

        return true;
    }
}
