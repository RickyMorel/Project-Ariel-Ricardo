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
    [SerializeField] private ResistanceType[] _resistanceType;
    [SerializeField] private WeaknessType[] _weaknessType;

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

        _lastRoutine = StartCoroutine(Afterburn());

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

    public virtual void Damage(int damage, DamageType damageType)
    {
        //0 is none, 1 is resistant, 2 is weak
        int isFireResistant = 0;

        for (int i = 0; i < _resistanceType.Length; i++)
        {
            if (((int)_resistanceType[i] == (int)damageType) && _resistanceType[i] != ResistanceType.None)
            {
                damage = damage / 2;

                isFireResistant = 1;
            }
        }

        for (int i = 0; i < _weaknessType.Length; i++)
        {
            if (((int)_weaknessType[i] == (int)damageType) && _weaknessType[i] != WeaknessType.None)
            {
                damage = damage * 2;

                if (isFireResistant == 1) { isFireResistant = 0; }

                else { isFireResistant = 2; }
            }
        }

        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);

        UpdateHealthUI();

        OnDamaged?.Invoke();

        if (_damageParticles != null) { _damageParticles.Play(); }

        if (DamageType.Electric == damageType) { _electricParticles.Play(); }

        if (DamageType.Fire == damageType) { _fireParticles.Play(); }

        if (isFireResistant == 0 && damageType == DamageType.Fire) { StopCoroutine(Afterburn(8)); StartCoroutine(Afterburn(8)); }

        if (isFireResistant == 1 && damageType == DamageType.Fire) { StopCoroutine(Afterburn(4)); StartCoroutine(Afterburn(4)); }

        if (isFireResistant == 2 && damageType == DamageType.Fire) { StopCoroutine(Afterburn(12)); StartCoroutine(Afterburn(12)); }

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

    private IEnumerator Afterburn(int damage)
    {
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
}