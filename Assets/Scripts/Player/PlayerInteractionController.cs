
using UnityEngine;

public class PlayerInteractionController : BaseInteractionController
{
    #region Private Variables

    private PlayerInputHandler _playerInput;

    #endregion

    #region Public Properties

    public PlayerInputHandler PlayerInput => _playerInput;

    #endregion

    #region Unity Loops

    public override void Start()
    {
        base.Start();

        _playerInput = GetComponent<PlayerInputHandler>();

        _playerInput.OnInteract += HandleInteraction;
        _playerInput.OnJump += HandleJump;
        _playerHealth.OnHurt += HandleHurt;
    }

    public override void Update()
    {
        base.Update();

        MoveDirection = _playerInput.MoveDirection;
        IsUsing = _playerInput.IsShooting;
    }

    private void OnDestroy()
    {
        _playerInput.OnInteract -= HandleInteraction;
        _playerInput.OnJump -= HandleJump;
        _playerHealth.OnHurt -= HandleHurt;
    }

    #endregion
}
