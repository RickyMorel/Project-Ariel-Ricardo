using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdollState : PlayerBaseState
{
    public PlayerRagdollState(BaseStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) 
    {
        _isRootState = true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        _context.CanMove = false;
        _context.PlayerRagdoll.EnableRagdoll(true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() 
    {
        _context.CanMove = true;
        _context.PlayerRagdoll.EnableRagdoll(false);
    }

    public override void InitializeSubStates() { }

    public override void CheckSwitchStates()
    {
        if(_context.PlayerHealth.IsHurt == false)
        {
            SwitchState(_factory.Grounded());
        }
    }
}
