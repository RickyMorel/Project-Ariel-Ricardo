using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState() 
    {
        _context.Speed = _context.RunSpeed;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() { }

    public override void InitializeSubStates() { }

    public override void CheckSwitchStates()
    {
        if (_context.IsShooting)
        {
            SwitchState(_factory.Attack());
        }
        else if (_context.IsDashPressed)
        {
            SwitchState(_factory.Dash());
        }
        else if(_context.MoveDirection.magnitude == 0)
        {
            SwitchState(_factory.Idle());
        }
    }
}
