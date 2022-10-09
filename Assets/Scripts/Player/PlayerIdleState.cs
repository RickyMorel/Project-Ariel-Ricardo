using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState() 
    {
        _context.Speed = 2;
    }

    public override void UpdateState() 
    {
        _context.Speed = 2;

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
        else if(_context.MoveDirection.magnitude > 0f)
        {
            SwitchState(_factory.Run());
        }
    }
}
