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

    [SerializeField] [Range(0f, 1f)] private float _lerpTime;

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
    [SerializeField] private Renderer _colorChange;
    [ColorUsageAttribute(false, true), SerializeField] private Color _redHDR;

    #endregion

    #region Private Variables

    private float _currentHealth;

    private Color _originalColor;

    private Coroutine _fireRoutine = null;
    private Coroutine _laserRoutine = null;
    private Coroutine _electricRoutine = null;

    private float _laserTimer = 0;
    private float _laserCountdown = 1;
    private int _laserLevel;
    private bool _isLerping = false;

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

        _originalColor = _colorChange.material.color;
    }

    private void Update()
    {
        _laserTimer = _laserTimer + Time.deltaTime;
        _laserCountdown = _laserTimer - Time.deltaTime;

        ColorChangeForLaser();
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

        finalDamage = DamageEffects(damageType, isResistant, isWeak, finalDamage);

        _currentHealth = Mathf.Clamp(_currentHealth - finalDamage, 0, _maxHealth);

        UpdateHealthUI();

        OnDamaged?.Invoke();

        if (_damageParticles != null) { _damageParticles.Play(); }

        if (_currentHealth == 0)
            Die();
    }

    private int DamageEffects(DamageType damageType, bool isResistant, bool isWeak, int finalDamage)
    {
        if (DamageType.Electric == damageType) { _electricParticles.Play(); }

        if (DamageType.Fire == damageType){ FireEffect(isResistant, isWeak); }

        if (DamageType.Laser == damageType) 
        { 
            finalDamage = LaserEffect(isResistant, isWeak, finalDamage);
        }

        return finalDamage;
    }

    private int LaserEffect(bool isResistant, bool isWeak, int finalDamage)
    {
        int laserDamage = 0;

        if (_laserTimer <= 2)
        {
            _laserTimer = 0;

            if (_laserLevel < 5) { _laserLevel -= -1; }

            _isLerping = true;

            if (_laserRoutine != null) { StopCoroutine(_laserRoutine); }

            _laserRoutine = StartCoroutine(ReturnToOriginalColor());

            if ((!isResistant && !isWeak) || (isResistant && isWeak)) { laserDamage = 8 * _laserLevel; }

            if (isResistant && !isWeak) { laserDamage = 4 * _laserLevel; }

            if (isWeak && !isResistant) { laserDamage = 12 * _laserLevel; }

            if (laserDamage == 0) { return finalDamage; }

            finalDamage = finalDamage + laserDamage;
        }
        else
        {
            _laserLevel = 1;
            _laserTimer = 0;

            _isLerping = true;

            if (_laserRoutine != null) { StopCoroutine(_laserRoutine); }

            _laserRoutine = StartCoroutine(ReturnToOriginalColor());

            if ((!isResistant && !isWeak) || (isResistant && isWeak)) { laserDamage = 8; }

            if (isResistant && !isWeak) { laserDamage = 4; }

            if (isWeak && !isResistant) { laserDamage = 12; }

            if (laserDamage == 0) { return finalDamage; }

            finalDamage = finalDamage + laserDamage;
        }
        return finalDamage;
    }

    private void ColorChangeForLaser()
    {
        if (!_isLerping) { return; }

        _colorChange.material.color = Color.Lerp(_colorChange.material.color, _redHDR, _laserTimer);

        if ((float)_laserLevel/5 <= _laserTimer) { _isLerping = false; }
    }

    private IEnumerator ReturnToOriginalColor()
    {
        yield return new WaitForSeconds(1);
        _laserCountdown = 1;
        _colorChange.material.color = Color.Lerp(_colorChange.material.color, _originalColor, -0.1f*_laserCountdown);
    }

    private void FireEffect(bool isResistant, bool isWeak)
    {
        _fireParticles.Play();

        int fireDamage = 0;

        if ((!isResistant && !isWeak) || (isResistant && isWeak)) { fireDamage = 8; }

        if (isResistant && !isWeak) { fireDamage = 4; }

        if (isWeak && !isResistant) { fireDamage = 12; }

        if (fireDamage == 0) { return; }

        if (_fireRoutine != null) { StopCoroutine(_fireRoutine); }

        _fireRoutine = StartCoroutine(Afterburn(fireDamage));
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
        if(_healthBarImage == null) { return; }

        _healthBarImage.fillAmount = CurrentHealth / MaxHealth;
    }

    #endregion
}