using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState() 
    {
        if (!_context.IsGrounded) { SetIsFalling(true); }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        SetIsFalling(false);
    }

    public override void InitializeSubStates() { }

    public override void CheckSwitchStates() 
    {
        if (_context.IsGrounded)
        {
            SwitchState(_factory.Grounded());
        }
    }

    private void HandleJump()
    {
        if (_context.PlayerInteraction.HasRecentlyInteracted()) { return; }

        _context.Anim.SetTrigger("Jump");

        float jumpingVelocity = Mathf.Sqrt(-2 * _context.GravityIntensity * _context.JumpHeight);
        Vector3 playerVelocity = _context.MoveDirection;
        playerVelocity.y = jumpingVelocity;
        _context.Rb.velocity = playerVelocity;
    }

    private void SetIsFalling(bool isFalling)
    {
        _context.Anim.SetBool("isFalling", isFalling);

        //Don't play jump anim while in air 
        if (isFalling) { _context.Anim.ResetTrigger("Jump"); }
    }
}
