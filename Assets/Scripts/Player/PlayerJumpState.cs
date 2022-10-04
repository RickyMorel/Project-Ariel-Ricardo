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

    public override void UpdateState() { }

    public override void ExitState()
    {
        if (_context.TimeSinceLastJump > _context.TimeSinceLastJump) { _context.CanJump = true; }

        _context.Anim.SetBool("isFalling", !_context.IsGrounded);

        //Don't play jump anim while in air 
        if (!_context.IsGrounded) { _context.Anim.ResetTrigger("Jump"); }
    }

    public override void InitializeSubStates() { }

    public override void CheckSwitchStates() { }

    private void HandleJump()
    {
        if (_context.PlayerInteraction.HasRecentlyInteracted()) { return; }

        _context.Anim.SetTrigger("Jump");

        _context.CanJump = false;

        _context.TimeSinceLastJump = 0f;

        float jumpingVelocity = Mathf.Sqrt(-2 * _context.GravityIntensity * _context.JumpHeight);
        Vector3 playerVelocity = _context.MoveDirection;
        playerVelocity.y = jumpingVelocity;
        _context.Rb.velocity = playerVelocity;
    }
}
