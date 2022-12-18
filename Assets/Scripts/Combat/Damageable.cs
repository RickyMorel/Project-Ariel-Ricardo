using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
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
    [SerializeField] private Renderer _colorChange;
    [ColorUsageAttribute(false, true), SerializeField] private Color _redHDR;

    #endregion

    #region Private Variables

    private float _currentHealth;

    [ColorUsageAttribute(false, true)] private Color _originalColor;

    private Coroutine _fireRoutine = null;
    private Coroutine _electricRoutine = null;

    private float _timeSinceLastLaserShot = 0;
    private float _timeToResetLaserLevel = 2;
    private float _laserLevel;
    private bool _isBeingElectrocuted = false;

    private ParticleSystem _fireParticles;
    private ParticleSystem _electricParticles;

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

        _colorChange.material.EnableKeyword("_EMISSION");

        _originalColor = _colorChange.material.GetColor("_EmissionColor");

        InstantiateDamageTypeParticles();
    }

    private void Update()
    {
        _timeSinceLastLaserShot += Time.deltaTime;

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

    private void InstantiateDamageTypeParticles()
    {
        GameObject fireParticlesInstance = Instantiate(GameAssetsManager.Instance.FireParticles, transform);
        GameObject electricParticleInstance = Instantiate(GameAssetsManager.Instance.ElectricParticles, transform);

        _fireParticles = fireParticlesInstance.GetComponent<ParticleSystem>();
        _electricParticles = electricParticleInstance.GetComponent<ParticleSystem>();

        _fireParticles.Stop();
        _electricParticles.Stop();
    }

    public void AddHealth(int amountAdded)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amountAdded, 0, _maxHealth);

        UpdateHealthUI();

        OnUpdateHealth?.Invoke();
    }

    public virtual void Damage(int damage, DamageType damageType = DamageType.None, bool isElectricChain = false)
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

        finalDamage = DamageTypesSelector(damageType, isResistant, isWeak, finalDamage, isElectricChain);

        _currentHealth = Mathf.Clamp(_currentHealth - finalDamage, 0, _maxHealth);

        UpdateHealthUI();

        OnDamaged?.Invoke();

        if (_damageParticles != null) { _damageParticles.Play(); }

        if (_currentHealth == 0)
            Die();
    }

    private int DamageTypesSelector(DamageType damageType, bool isResistant, bool isWeak, int finalDamage, bool isElectricChain)
    {
        if (DamageType.Electric == damageType) { ElectricDamage(isElectricChain); }

        if (DamageType.Fire == damageType){ FireDamage(isResistant, isWeak); }

        if (DamageType.Laser == damageType) { finalDamage = LaserDamage(isResistant, isWeak, finalDamage); }

        return finalDamage;
    }

    private int LaserDamage(bool isResistant, bool isWeak, int finalDamage)
    {
        int laserDamage = 0;

        _laserLevel = Mathf.Clamp(_laserLevel + 0.3f, 0f, 5f);

        _timeSinceLastLaserShot = 0;

        if ((!isResistant && !isWeak) || (isResistant && isWeak)) { laserDamage = 8 * (int)_laserLevel; }

        if (isResistant && !isWeak) { laserDamage = 4 * (int)_laserLevel; }

        if (isWeak && !isResistant) { laserDamage = 12 * (int)_laserLevel; }

        if (laserDamage == 0) { return finalDamage; }

        finalDamage = finalDamage + laserDamage;

        return finalDamage;
    }

    private void ColorChangeForLaser()
    {
        if (_colorChange == null) { return; }
        
        if (_timeSinceLastLaserShot > _timeToResetLaserLevel)
        {
            float laserReductionAmount = 2.5f * Time.deltaTime;
            _laserLevel = Mathf.Clamp(_laserLevel - laserReductionAmount, 0, 5);
        }

        _colorChange.material.SetColor("_EmissionColor", Color.Lerp(_originalColor, _redHDR, _laserLevel/5f));
    }

    private void FireDamage(bool isResistant, bool isWeak)
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

    private void ElectricDamage(bool isElectricChain)
    {
        if (_isBeingElectrocuted && isElectricChain) { return; }

        _isBeingElectrocuted = true;

        if (TryGetComponent<BaseStateMachine>(out BaseStateMachine baseStateMachine)) { baseStateMachine.CanMove = false; }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (var hitCollider in hitColliders)
        {
            if (!hitCollider.TryGetComponent<Damageable>(out Damageable damageable)) { continue; }

            damageable.Damage(200, DamageType.Electric, true);
        }

        if (_electricRoutine != null) { StopCoroutine(_electricRoutine); }

        _electricRoutine = StartCoroutine(ElectricParalysis(baseStateMachine));
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

    private IEnumerator ElectricParalysis(BaseStateMachine baseStateMachine)
    {
        _electricParticles.Play();
        yield return new WaitForSeconds(2);
        baseStateMachine.CanMove = true;
        _isBeingElectrocuted = false;
        _electricParticles.Stop();
    }

    #region UI

    public void UpdateHealthUI()
    {
        if(_healthBarImage == null) { return; }

        _healthBarImage.fillAmount = CurrentHealth / MaxHealth;
    }

    #endregion
}