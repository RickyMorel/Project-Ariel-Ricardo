using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarryState : PlayerBaseState
{
    public PlayerCarryState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState() 
    {
        _context.Speed = _context.PlayerCarryController.CarryWalkSpeed;
        _context.Anim.SetBool("Carry", true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() 
    {
        _context.PlayerCarryController.DropAllItems();
        _context.Anim.SetBool("Carry", false);
    }

    public override void InitializeSubStates() { }

    public override void CheckSwitchStates()
    {
        if (_context.IsShooting)
        {
            SwitchState(_factory.Attack());
        }
        else if(_context.PlayerCarryController.HasItems == false)
        {
            SwitchState(_factory.Idle());
        }
    }
}
