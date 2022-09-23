using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Animator _anim;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteractionController _playerInteractionController;

    #endregion

    #region Public Properties

    public Rigidbody Rb => _rb;
    public Animator Anim => _anim;
    public PlayerInputHandler PlayerInputHandler => _playerInputHandler;
    public PlayerMovement PlayerMovement => _playerMovement;
    public PlayerInteractionController PlayerInteractionController => _playerInteractionController;

    #endregion
}
