using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private List<Collider> _colliders = new List<Collider>();

    #endregion

    #region Private Variables

    private Rigidbody _mainRb;
    private CapsuleCollider _mainCollider;
    private Animator _anim;

    #endregion

    #region Unity Loops

    private void Awake()
    {
        _mainRb = GetComponent<Rigidbody>();
        _mainCollider = GetComponent<CapsuleCollider>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        EnableRagdoll(false);
    }

    #endregion

    public void EnableRagdoll(bool isEnabled)
    {
        if(isEnabled == false)
        {
            //transform.position = _colliders[0].transform.position;
        }

        _mainRb.useGravity = !isEnabled;
        _mainCollider.enabled = !isEnabled;
        _anim.enabled = !isEnabled;


        foreach (Collider collider in _colliders)
        {
            collider.enabled = isEnabled;
        }
    }
}
