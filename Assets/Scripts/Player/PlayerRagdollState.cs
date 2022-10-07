using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdollState : PlayerBaseState
{
    public PlayerRagdollState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) 
    {
        _isRootState = true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        _context.PlayerRagdoll.EnableRagdoll(true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() 
    {
        _context.PlayerRagdoll.EnableRagdoll(false);
    }

    public override void InitializeSubStates() { }

    public override void CheckSwitchStates()
    {
        if (_context.IsJumpPressed)
        {
            SwitchState(_factory.Jump());
        }
        else if (!_context.IsGrounded)
        {
            SwitchState(_factory.Fall());
        }
    }
}
