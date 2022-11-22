using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Damageable
{
    #region Editor Fields

    [SerializeField] private bool _canKill = false;
    [SerializeField] private float _hurtTime = 5f;

    #endregion

    #region Private Variables

    private bool _isHurt;

    #endregion

    #region Public Properties

    public bool IsHurt => _isHurt;

    public event Action OnHurt;

    #endregion

    //This is for the child classes
    public virtual void Start()
    {
        base.Start();
    }

    public virtual void Hurt()
    {
        OnHurt?.Invoke();

        StartCoroutine(HurtCoroutine());
    }

    private IEnumerator HurtCoroutine()
    {
        _isHurt = true;

        yield return new WaitForSeconds(_hurtTime);

        _isHurt = false;
    }
}
