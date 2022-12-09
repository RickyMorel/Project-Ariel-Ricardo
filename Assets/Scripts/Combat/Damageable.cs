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

    [Header("Type Resistances")]
    [SerializeField] private List<DamageType> _resistanceType = new List<DamageType>();
    [SerializeField] private List<DamageType> _weaknessType = new List<DamageType>();

    [Header("Stats")]
    [SerializeField] private int _maxHealth;

    [Header("UI")]
    [SerializeField] private Image _healthBarImage;

    [Header("FX")]
    [SerializeField] private ParticleSystem _damageParticles;
    [SerializeField] private ParticleSystem _fireParticles;
    [SerializeField] private ParticleSystem _electricParticles;

    #endregion

    #region Private Variables

    private float _currentHealth;

    private Coroutine _lastRoutine = null;

    #endregion

    #region Getters and Setters

    public float CurrentHealth => _currentHealth;

    #endregion

    #region Public Properties

    public float MaxHealth => _maxHealth;

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

        Damage(projectile.Damage, projectile.DamageType);

        Destroy(projectile.gameObject);
    }

    #endregion

    public void AddHealth(int amountAdded)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amountAdded, 0, _maxHealth);

        UpdateHealthUI();

        OnUpdateHealth?.Invoke();
    }

    public virtual void Damage(int damage, DamageType damageType = DamageType.None)
    {
        int finalDamage = damage;

        bool isWeak = false;
        bool isResistant = false;

        if (_resistanceType.Contains(damageType)) 
        { 
            finalDamage = finalDamage / 2;
            isResistant = true;
        }

        if (_weaknessType.Contains(damageType)) 
        { 
            finalDamage = finalDamage * 2;
            isWeak = true;
        }

        _currentHealth = Mathf.Clamp(_currentHealth - finalDamage, 0, _maxHealth);

        UpdateHealthUI();

        OnDamaged?.Invoke();

        if (_damageParticles != null) { _damageParticles.Play(); }

        DamageEffects(damageType, isResistant, isWeak);

        if (_currentHealth == 0)
            Die();
    }

    private void DamageEffects(DamageType damageType, bool isResistant, bool isWeak)
    {
        if (DamageType.Electric == damageType) { _electricParticles.Play(); }

        if (DamageType.Fire == damageType)
        {
            _fireParticles.Play();

            if ((!isResistant && !isWeak) || (isResistant && isWeak)) { StopCoroutine(_lastRoutine); _lastRoutine = StartCoroutine(Afterburn(8)); }

            if (isResistant && !isWeak) { StopCoroutine(_lastRoutine); _lastRoutine = StartCoroutine(Afterburn(4)); }

            if (isWeak && !isResistant) { StopCoroutine(_lastRoutine); _lastRoutine = StartCoroutine(Afterburn(12)); }
        }
    }

    public virtual void Die()
    {
        OnDie?.Invoke();
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        _maxHealth = newMaxHealth;
    }

    private IEnumerator Afterburn(int damage)
    {
        Debug.Log("Hell");
        yield return new WaitForSeconds(1);
        Damage(damage, DamageType.None);
        yield return new WaitForSeconds(1);
        Damage(damage, DamageType.None);
        yield return new WaitForSeconds(1);
        Damage(damage, DamageType.None);
        yield return new WaitForSeconds(1);
        Damage(damage, DamageType.None);
        yield return new WaitForSeconds(1);
        Damage(damage, DamageType.None);
        _fireParticles.Stop();
    }

    #region UI

    public void UpdateHealthUI()
    {
        _healthBarImage.fillAmount = CurrentHealth / MaxHealth;
    }

    #endregion
}