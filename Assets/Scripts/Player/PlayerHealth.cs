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

    #endregion

    public void Hurt()
    {
        StartCoroutine(HurtCoroutine());
    }

    private IEnumerator HurtCoroutine()
    {
        _isHurt = true;

        yield return new WaitForSeconds(_hurtTime);

        _isHurt = false;
    }
}
