using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Damageable : MonoBehaviour
{
    #region Editor Fields

    [Header("Stats")]
    [SerializeField] private int _maxHealth;

    [Header("UI")]
    [SerializeField] private Image _healthBarImage;

    [Header("FX")]
    [SerializeField] private ParticleSystem _damageParticles;

    #endregion

    #region Private Variables

    private float _currentHealth;

    #endregion

    #region Getters and Setters

    public float CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

    #endregion

    #region Public Properties

    public float MaxHealth => _maxHealth;
    public ParticleSystem DamageParticles => _damageParticles;


    public event Action OnUpdateHealth;
    public event Action OnDamaged;
    public event Action OnDie;

    #endregion

    #region Unity Loops

    public virtual void Start()
    {
        _currentHealth = _maxHealth;

        UpdateHealthUI();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<Projectile>(out Projectile projectile)) { return; }

        //Turrets can't harm their own ship
        if(other.gameObject.tag == "Untagged" && gameObject.tag == "MainShip") { return; }

        if(other.gameObject.tag == gameObject.tag) { return; }

        Damage(projectile.Damage);

        Destroy(projectile.gameObject);
    }

    #endregion

    public void AddHealth(int amountAdded)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amountAdded, 0, _maxHealth);

        UpdateHealthUI();

        OnUpdateHealth?.Invoke();
    }

    public virtual void Damage(int damage)
    {

        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);

        UpdateHealthUI();

        OnDamaged?.Invoke();

        if (_damageParticles != null) { _damageParticles.Play(); }

        if (_currentHealth == 0)
            Die();
    }

    public virtual void Die()
    {
        OnDie?.Invoke();
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        _maxHealth = newMaxHealth;
    }

    #region UI

    public void UpdateHealthUI()
    {
        _healthBarImage.fillAmount = CurrentHealth / MaxHealth;
    }

    #endregion
}