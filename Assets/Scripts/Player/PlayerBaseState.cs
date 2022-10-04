
public abstract class PlayerBaseState
{
    protected PlayerStateMachine _context;
    protected PlayerStateFactory _factory;
    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _context = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubStates();

    private void UpdateStates() { }
    protected void SwitchState(PlayerBaseState newState)
    {
        //current state exits state
        ExitState();

        //new state enters state
        newState.EnterState();

        //switch current state of context
        _context.CurrentState = newState;
    }
    protected void SetSuperState() { }
    protected void SetSubState() { }
}
