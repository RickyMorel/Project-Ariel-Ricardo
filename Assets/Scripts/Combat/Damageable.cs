using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private int _maxHealth;

    #endregion

    #region Private Variables

    private int _currentHealth;

    #endregion

    #region Public Properties

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;

    public event Action OnUpdateHealth;
    public event Action OnDamaged;
    public event Action OnDie;

    #endregion

    #region Unity Loops

    public virtual void Start()
    {
        _currentHealth = _maxHealth / 2;
    }

    #endregion

    public void AddHealth(int amountAdded)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amountAdded, 0, _maxHealth);

        OnUpdateHealth?.Invoke();
    }

    public void Damage(int damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);

        OnDamaged?.Invoke();

        if (_currentHealth == 0)
            OnDie?.Invoke();
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        _maxHealth = newMaxHealth;
    }
}
