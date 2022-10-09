using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    #region Private Variables

    private float _timeSinceLastDash;
    private float _timeBetweenDashes = 0.5f;

    #endregion

    public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        _context.Speed = 0;

        Dash();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        _timeSinceLastDash += Time.deltaTime;
    }

    public override void ExitState() { }

    public override void InitializeSubStates() { }

    public override void CheckSwitchStates()
    {
        if(_timeSinceLastDash < _timeBetweenDashes) { return; }

        SwitchState(_factory.Idle());
    }

    private void Dash()
    {
        _timeSinceLastDash = 0f;

        _context.Anim.SetTrigger("Dash");

        _context.Rb.AddForce(_context.MoveDirection.normalized * 20, ForceMode.Impulse);
    }
}
