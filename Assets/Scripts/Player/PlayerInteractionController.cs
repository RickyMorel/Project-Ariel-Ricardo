
public class PlayerInteractionController : BaseInteractionController
{
    #region Private Variables

    private PlayerInputHandler _playerInput;
    private ShipInventory _shipInventory;

    #endregion

    #region Public Properties

    public PlayerInputHandler PlayerInput => _playerInput;

    #endregion

    #region Unity Loops

    public override void Start()
    {
        base.Start();

        _playerInput = GetComponent<PlayerInputHandler>();
        _shipInventory = FindObjectOfType<ShipInventory>();

        _playerInput.OnInteract += HandleInteraction;
        _playerInput.OnUpgrade += HandleUpgrade;
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
        _playerInput.OnUpgrade -= HandleUpgrade;
        _playerInput.OnJump -= HandleJump;
        _playerHealth.OnHurt -= HandleHurt;
    }

    #endregion


    //This calls when the player presses the upgrade button
    private void HandleUpgrade()
    {
        if (_currentInteractable == null) { return; }

        if((_currentInteractable is Upgradable) == false) { return; }

        Upgradable upgradable = _currentInteractable as Upgradable;
        upgradable.TryUpgrade(_shipInventory.InventoryDictionary);
    }
}
