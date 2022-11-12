using System;
using UnityEngine;
using Rewired;

public class PlayerInputHandler : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private int _playerId = 0;

    #endregion

    #region Private Variables

    private Player _player;

    private Vector2 _moveDirection;
    private bool _isShooting;
    private bool _canPlayerSpawn = false;

    #endregion

    #region Public Properties

    public bool IsPlayerActive = true;

    public static event Action<PlayerInputHandler, bool> OnSpecialAction;

    public event Action OnJump;
    public event Action OnInteract;
    public event Action OnConfirm;
    public event Action OnCancel;
    public event Action OnUpgrade;
    public event Action<PlayerInputHandler> OnTrySpawn;
   
    public Vector2 MoveDirection => _moveDirection;
   
    public bool IsShooting => _isShooting;

    #endregion

    #region Getters And Setters

    public bool CanPlayerSpawn { get { return _canPlayerSpawn; } set { _canPlayerSpawn = value; } }

    #endregion

    private void Start()
    {
        _player = ReInput.players.GetPlayer(_playerId);
    }

    private void Update()
    {
        Move();
        Jump();
    }

    public void Move()
    {
        if (!IsPlayerActive) { return; }

        float moveHorizontal = _player.GetAxisRaw("Horizontal");
        float moveVertical = _player.GetAxisRaw("Vertical");

        _moveDirection = new Vector2(moveHorizontal, moveVertical).normalized;
    }

    public void Jump()
    {
        if (!_player.GetButtonDown("Jump")) { return; }

        if (CanPlayerSpawn)
        {
            OnTrySpawn?.Invoke(this);
        }

        else if (IsPlayerActive)
        {
            OnJump?.Invoke();
        }
    }

    //public void Confirm(InputAction.CallbackContext obj)
    //{
    //    if (!IsPlayerActive) { return; }

    //    //prevents from spam calling this function
    //    if (!obj.started) { return; }

    //    OnConfirm?.Invoke();
    //}

    //public void Cancel(InputAction.CallbackContext obj)
    //{
    //    if (!IsPlayerActive) { return; }

    //    //prevents from spam calling this function
    //    if (!obj.started) { return; }

    //    OnCancel?.Invoke();
    //}

    //public void SpecialAction(InputAction.CallbackContext obj)
    //{
    //    if (!IsPlayerActive) { return; }

    //    //prevents from spam calling this function
    //    if (!obj.started) { return; }

    //    bool value = obj.ReadValue<float>() == 1f ? true : false;
    //    OnSpecialAction?.Invoke(this, value);
    //}

    //public void Interact(InputAction.CallbackContext obj)
    //{
    //    if (!IsPlayerActive) { return; }

    //    //prevents from spam calling this function
    //    if (!obj.started) { return; }

    //    OnInteract?.Invoke();
    //}

    //public void Upgrade(InputAction.CallbackContext obj)
    //{
    //    if (!IsPlayerActive) { return; }

    //    //prevents from spam calling this function
    //    if (!obj.started) { return; }

    //    OnUpgrade?.Invoke();
    //}

    //public void Shoot(InputAction.CallbackContext obj)
    //{
    //    if (!IsPlayerActive) { return; }

    //    _isShooting = obj.ReadValue<float>() == 1f ? true : false;
    //}
}