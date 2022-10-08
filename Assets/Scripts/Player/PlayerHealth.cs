using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private float _hurtTime = 5f;

    #endregion

    #region Private Variables

    private bool _isHurt;

    #endregion

    #region Public Properties

    public bool IsHurt => _isHurt;

    public event Action OnHurt;

    #endregion

    public void Hurt()
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
